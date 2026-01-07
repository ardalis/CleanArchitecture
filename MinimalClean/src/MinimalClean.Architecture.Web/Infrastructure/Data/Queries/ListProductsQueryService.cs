using Microsoft.EntityFrameworkCore;
using MinimalClean.Architecture.Web.ProductFeatures;
using MinimalClean.Architecture.Web.ProductFeatures.List;

namespace MinimalClean.Architecture.Web.Infrastructure.Data.Queries;

public class ListProductsQueryService(AppDbContext db) : IListProductsQueryService
{
  private readonly AppDbContext _db = db;

    public async Task<PagedResult<ProductDto>> ListAsync(int page, int perPage)
  {
    var items = await _db.Products
      .OrderBy(p => p.Id)
      .Skip((page - 1) * perPage)
      .Take(perPage)
      .Select(p => new ProductDto(p.Id, p.Name, p.UnitPrice))
      .AsNoTracking()
      .ToListAsync();

    int totalCount = await _db.Products.CountAsync();
    int totalPages = (int)Math.Ceiling(totalCount / (double)perPage);
    var result = new PagedResult<ProductDto>(items, page, perPage, totalCount, totalPages);

    return result;
  }
}
