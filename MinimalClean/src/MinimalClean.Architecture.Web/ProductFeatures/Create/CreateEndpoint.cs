using FastEndpoints;
using FluentValidation;
using MinimalClean.Architecture.Web.Infrastructure.Data;
using MinimalClean.Architecture.Web.Products;

namespace MinimalClean.Architecture.Web.ProductFeatures.List;

public sealed class CreateProductRequest
{ 
  public string Name { get; init; } = string.Empty;
  public decimal UnitPrice { get; init; }
}

// TODO: Add DbContext injection
// AppDbContext dbContext
public class CreateEndpoint() : 
  Endpoint<CreateProductRequest, ProductRecord>
{
  public override void Configure()
  {
    Post("/Products");
    AllowAnonymous();

    Tags("Products");

    Description(builder => builder
      .Accepts<CreateProductRequest>()
      .Produces<ProductRecord>(200, "application/json")
      .ProducesProblem(400));
  }

  public override async Task HandleAsync(CreateProductRequest request,
    CancellationToken cancellationToken)
  {
    // TODO: Use DbContext to create product
  }

}
