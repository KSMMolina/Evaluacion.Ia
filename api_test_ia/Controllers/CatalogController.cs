using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Application.UseCases.Categories.Queries;
using Api_test_ia.Application.UseCases.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api_test_ia.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/catalog")]
    public sealed class CatalogController(IMediator mediator) : ControllerBase
    {
        // Productos visibles al público (solo activos)
        [HttpGet("products")]
        public async Task<ActionResult<PagedResult<ProductListItemDto>>> List(
            [FromQuery] string? search, [FromQuery] int? categoryId,
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice,
            [FromQuery] string? sort, [FromQuery] string? dir,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 12, CancellationToken ct = default)
        {
            var result = await mediator.Send(new ListProductsQuery(
                search, categoryId, true, sort, dir, page, pageSize, minPrice, maxPrice), ct);

            Response.Headers["X-Total-Count"] = result.Total.ToString();
            return Ok(result);
        }

        [HttpGet("products/{id:int}")]
        public async Task<ActionResult<ProductDetailDto>> Detail(int id, CancellationToken ct)
        {
            var p = await mediator.Send(new GetProductDetailQuery(id), ct);
            if (p is null || !p.IsActive) return NotFound();
            return Ok(p);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryNodeDto>>> Categories([FromQuery] bool flat = false, CancellationToken ct = default)
            => Ok(await mediator.Send(new GetPublicCategoriesTreeQuery(flat), ct));
    }
}
