using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalClean.Architecture.Web.Domain.CartAggregate;
using MinimalClean.Architecture.Web.Domain.CartAggregate.Specifications;

namespace MinimalClean.Architecture.Web.CartFeatures.UpdateItemQuantity;

public sealed class UpdateCartItemRequest
{
  public const string Route = "/cart/{CartId}/items/{ProductId}";

  public Guid CartId { get; init; }
  public int ProductId { get; init; }
  public int Quantity { get; init; }
}

public class UpdateCartItemEndpoint(IMediator mediator, IRepository<Cart> cartRepository)
  : FastEndpoints.Endpoint<UpdateCartItemRequest,
             Results<Ok<CartResponse>,
                     NotFound,
                     ValidationProblem,
                     ProblemHttpResult>,
             UpdateCartItemMapper>
{
  public override void Configure()
  {
    Put(UpdateCartItemRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Update cart item quantity";
      s.Description = "Updates the quantity of an existing item in the cart. Returns the updated cart with all items.";
      s.ExampleRequest = new UpdateCartItemRequest
      {
        CartId = Guid.Parse("12345678-1234-1234-1234-123456789012"),
        ProductId = 1,
        Quantity = 3
      };
      s.ResponseExamples[200] = new CartResponse(
        Guid.Parse("12345678-1234-1234-1234-123456789012"),
        new List<CartItemResponse>
        {
          new(1, 3, 999.99m, 2999.97m)
        },
        2999.97m);

      // Document possible responses
      s.Responses[200] = "Item quantity updated successfully";
      s.Responses[404] = "Cart or item not found";
      s.Responses[400] = "Invalid request data";
    });

    // Add tags for API grouping
    Tags("Cart");

    // Add additional metadata
    Description(builder => builder
      .Accepts<UpdateCartItemRequest>("application/json")
      .Produces<CartResponse>(200, "application/json")
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<Ok<CartResponse>, NotFound, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(UpdateCartItemRequest request, CancellationToken ct)
  {
    var cartId = CartId.From(request.CartId);

    // Check the cart exists before sending the command
    var cart = await cartRepository.FirstOrDefaultAsync(new CartByIdSpec(cartId), ct);
    if (cart == null)
    {
      return TypedResults.NotFound();
    }

    // Quantity of 0 means the customer is removing the item, so we can just
    // update it directly here instead of going through the command pipeline
    if (request.Quantity == 0)
    {
      var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
      if (item != null)
      {
        item.Quantity = 0;
        await cartRepository.UpdateAsync(cart, ct);
      }

      var itemResponses = cart.Items.Select(i => new CartItemResponse(
        i.ProductId,
        i.Quantity,
        i.UnitPrice,
        i.Quantity * i.UnitPrice
      )).ToList();

      return TypedResults.Ok(new CartResponse(cart.Id.Value, itemResponses, itemResponses.Sum(i => i.TotalPrice)));
    }

    var command = new UpdateCartItemCommand(cartId, request.ProductId, request.Quantity);
    var result = await mediator.Send(command, ct);

    if (result.Status == ResultStatus.NotFound)
    {
      return TypedResults.NotFound();
    }

    if (!result.IsSuccess)
    {
      return TypedResults.Problem(result.Errors.FirstOrDefault() ?? "An error occurred");
    }

    var response = Map.FromEntity(result.Value);
    return TypedResults.Ok(response);
  }
}

public sealed class UpdateCartItemValidator : FastEndpoints.Validator<UpdateCartItemRequest>
{
  public UpdateCartItemValidator()
  {
    RuleFor(x => x.ProductId)
      .GreaterThan(0)
      .WithMessage("Product ID must be greater than 0");

    RuleFor(x => x.Quantity)
      .LessThanOrEqualTo(100)
      .WithMessage("Quantity cannot exceed 100");
  }
}

public sealed class UpdateCartItemMapper
  : FastEndpoints.Mapper<UpdateCartItemRequest, CartResponse, CartDto>
{
  public override CartResponse FromEntity(CartDto e)
  {
    var items = e.Items.Select(i => new CartItemResponse(
      i.ProductId,
      i.Quantity,
      i.UnitPrice,
      i.TotalPrice
    )).ToList();

    return new CartResponse(e.Id.Value, items, e.Total);
  }
}
