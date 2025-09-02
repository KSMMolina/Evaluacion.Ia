using Api_test_ia.Application.Dtos;
using Api_test_ia.Application.UseCases.Categories.Commands;
using Api_test_ia.Application.UseCases.Categories.Queries;
using Api_test_ia.Presentation.Contracts.Admin.Categories;
using Api_test_ia.Presentation.Mappings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_test_ia.Presentation.Controllers.Admin
{
    [ApiController]
    [Authorize(Policy = "AdminOrEditor")]
    [Route("api/v1/admin/categories")]
    public class AdminCategoriesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CategoryNodeDto>>> List([FromQuery] bool flat = false, CancellationToken ct = default)
            => Ok(await mediator.Send(new GetAdminCategoriesTreeQuery(flat), ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest req, CancellationToken ct)
        {
            var id = await mediator.Send(req.ToCommand(), ct);
            return Created($"/api/v1/admin/categories/{id}", null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryRequest req, CancellationToken ct)
        {
            await mediator.Send(id.ToCommand(req), ct);
            return NoContent();
        }

        [HttpPatch("{id:int}/toggle")]
        public async Task<IActionResult> Toggle(int id, CancellationToken ct)
        {
            await mediator.Send(new ToggleCategoryCommand(id), ct);
            return NoContent();
        }
    }
}
