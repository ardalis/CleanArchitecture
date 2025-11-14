using MinimalClean.Architecture.Web.Domain.CartAggregate;
using MinimalClean.Architecture.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using FastEndpoints;

namespace MinimalClean.Architecture.Web.CartFeatures.GetById;

public sealed class GetCartRequest
{
  public const string Route = "/cart/{CartId}";

  public Guid CartId { get; init; }
}

public class GetByIdEndpoint(IMediator mediator)
  : Endpoint<GetCartRequest,
             Results<Ok<CartResponse>,
                     NotFound,
                     ProblemHttpResult>,
             GetCartMapper>
{
  public override void Configure()
  {
    Get(GetCartRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Get cart by ID";
      s.Description = "Retrieves a cart and all its items by cart ID.";
      s.ExampleRequest = new GetCartRequest 
      { 
        CartId = Guid.Parse("12345678-1234-1234-1234-123456789012")
      };
      s.ResponseExamples[200] = new CartResponse(
        Guid.Parse("12345678-1234-1234-1234-123456789012"),
        new List<CartItemResponse>
        {
          new(1, 2, 999.99m, 1999.98m)
        },
        1999.98m);

      // Document possible responses
      s.Responses[200] = "Cart retrieved successfully";
      s.Responses[404] = "Cart not found";
    });

    // Add tags for API grouping
    Tags("Cart");

    // Add additional metadata
    Description(builder => builder
      .Produces<CartResponse>(200, "application/json")
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<CartResponse>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetCartRequest request, CancellationToken ct)
  {
    var cartId = CartId.From(request.CartId);
    var query = new GetCartQuery(cartId);
    var result = await mediator.Send(query, ct);

    return result.ToGetByIdResult(Map.FromEntity);
  }
}

public sealed class GetCartMapper
  : Mapper<GetCartRequest, CartResponse, CartDto>
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
