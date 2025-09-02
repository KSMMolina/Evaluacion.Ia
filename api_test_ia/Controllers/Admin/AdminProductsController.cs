using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Application.UseCases.Products.Commands;
using Api_test_ia.Application.UseCases.Products.Queries;
using Api_test_ia.Presentation.Contracts.Admin.Products;
using Api_test_ia.Presentation.Mappings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_test_ia.Presentation.Controllers.Admin
{
    [ApiController]
    [Authorize(Policy = "AdminOrEditor")]
    [Route("api/v1/admin/products")]
    public class AdminProductsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductListItemDto>>> List(
            [FromQuery] string? search, [FromQuery] int? categoryId,
            [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice,
            [FromQuery] string? sort, [FromQuery] string? dir,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 12, CancellationToken ct = default)
        {
            var result = await mediator.Send(new ListProductsQuery(
                search, categoryId, false, sort, dir, page, pageSize, minPrice, maxPrice), ct);

            Response.Headers["X-Total-Count"] = result.Total.ToString();
            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDetailDto>> Get(int id, CancellationToken ct)
            => (await mediator.Send(new GetProductDetailQuery(id), ct)) is { } p ? Ok(p) : NotFound();


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest req, CancellationToken ct)
        {
            var id = await mediator.Send(req.ToCommand(), ct);
            return CreatedAtAction(nameof(Get), new { id }, null);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest req, CancellationToken ct)
        {
            await mediator.Send(id.ToCommand(req), ct);
            return NoContent();
        }


        [HttpPatch("{id:int}/toggle")]
        public async Task<IActionResult> Toggle(int id, CancellationToken ct)
        { await mediator.Send(new ToggleProductCommand(id), ct); return NoContent(); }

        // Images:

        [HttpPost("{id:int}/images")]
        public async Task<IActionResult> AddImage(int id, [FromBody] AddProductImageRequest body, CancellationToken ct)
        {
            var imgId = await mediator.Send(new AddProductImageCommand(id, body.Url, body.AltText, body.SortOrder), ct);
            return Created($"/api/v1/admin/products/{id}/images/{imgId}", null);
        }

        [HttpPost("{id:int}/images/upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> UploadImages(
            int id,
            [FromForm] List<IFormFile> files,
            CancellationToken ct)
        {
            if (files is null || files.Count == 0)
                return BadRequest("Debe enviar al menos un archivo.");

            var incoming = files.Select(f => new IncomingFile(
                f.FileName, f.ContentType, f.Length, f.OpenReadStream()
            )).ToList();

            var created = await mediator.Send(new UploadProductImagesCommand(id, incoming), ct);
            return Ok(created); // o 201 Created si prefieres
        }

        [HttpPost("{id:int}/images/{imgId:int}/replace")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(2_000_000)]
        public async Task<ActionResult<object>> ReplaceImage(int id, int imgId, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0) return BadRequest("Archivo vacío.");

            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);
            ms.Position = 0;

            var url = await mediator.Send(new ReplaceProductImageCommand(
                id, imgId, file.FileName, file.ContentType, ms), ct);

            return Ok(new { url });
        }

        [HttpDelete("{id:int}/images/{imgId:int}")]
        public async Task<IActionResult> DeleteImage(int id, int imgId, CancellationToken ct)
        {
            await mediator.Send(new DeleteProductImageCommand(id, imgId), ct);
            return NoContent();
        }
    }
}
