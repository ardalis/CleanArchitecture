using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalClean.Architecture.Web.Domain.CartAggregate;

namespace MinimalClean.Architecture.Web.CartFeatures.AddToCart;

public sealed class AddToCartRequest
{
  public const string Route = "/cart";

  public Guid? CartId { get; init; }
  public int ProductId { get; init; }
  public int Quantity { get; init; }
}


public class AddToCartEndpoint(IMediator mediator)
  : FastEndpoints.Endpoint<AddToCartRequest,
             Results<Ok<CartResponse>,
                     NotFound,
                     ValidationProblem,
                     ProblemHttpResult>,
             AddToCartMapper>
{
  public override void Configure()
  {
    Post(AddToCartRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Add item to cart";
      s.Description = "Adds a product to an existing cart or creates a new cart if CartId is not provided. Returns the updated cart with all items.";
      s.ExampleRequest = new AddToCartRequest 
      { 
        CartId = null, // null creates new cart
        ProductId = 1, 
        Quantity = 2 
      };
      s.ResponseExamples[200] = new CartResponse(
        Guid.Empty,
        new List<CartItemResponse>
        {
          new(1, 2, 999.99m, 1999.98m)
        },
        1999.98m);

      // Document possible responses
      s.Responses[200] = "Item added to cart successfully";
      s.Responses[404] = "Product or Cart not found";
      s.Responses[400] = "Invalid request data";
    });

    // Add tags for API grouping
    Tags("Cart");

    // Add additional metadata
    Description(builder => builder
      .Accepts<AddToCartRequest>("application/json")
      .Produces<CartResponse>(200, "application/json")
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<Ok<CartResponse>, NotFound, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(AddToCartRequest request, CancellationToken ct)
  {
    var cartId = request.CartId.HasValue ? CartId.From(request.CartId.Value) : (CartId?)null;
    var command = new AddToCartCommand(cartId, request.ProductId, request.Quantity);
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

public sealed class AddToCartValidator : FastEndpoints.Validator<AddToCartRequest>
{
  public AddToCartValidator()
  {
    RuleFor(x => x.ProductId)
      .GreaterThan(0)
      .WithMessage("Product ID must be greater than 0");

    RuleFor(x => x.Quantity)
      .GreaterThan(0)
      .WithMessage("Quantity must be greater than 0");
  }
}


public sealed class AddToCartMapper
  : FastEndpoints.Mapper<AddToCartRequest, CartResponse, CartDto>
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

