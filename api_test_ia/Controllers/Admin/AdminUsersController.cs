using Api_test_ia.Application.Common;
using Api_test_ia.Application.Dtos;
using Api_test_ia.Application.UseCases.Users.Commands;
using Api_test_ia.Application.UseCases.Users.Queries;
using Api_test_ia.Presentation.Contracts.Admin.Users;
using Api_test_ia.Presentation.Mappings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_test_ia.Presentation.Controllers.Admin
{
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    [Route("api/v1/admin/users")]
    public sealed class AdminUsersController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<UserDto>>> List(
            [FromQuery] string? search, [FromQuery] string? role,
            [FromQuery] string? sort, [FromQuery] string? dir,
            [FromQuery] int page = 1, [FromQuery] int pageSize = 12, CancellationToken ct = default)
        {
            var result = await mediator.Send(new ListUsersQuery(search, role, sort, dir, page, pageSize), ct);
            Response.Headers["X-Total-Count"] = result.Total.ToString();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDetailDto>> Get(int id, CancellationToken ct)
            => (await mediator.Send(new GetUserDetailQuery(id), ct)) is { } u ? Ok(u) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest req, CancellationToken ct)
        {
            var id = await mediator.Send(req.ToCommand(), ct);
            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest req, CancellationToken ct)
        {
            await mediator.Send(id.ToCommand(req), ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await mediator.Send(new DeleteUserCommand(id), ct);
            return NoContent();
        }
    }
}
