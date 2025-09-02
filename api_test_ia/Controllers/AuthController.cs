using MediatR;
using Microsoft.AspNetCore.Mvc;

using Api_test_ia.Application.Dtos;
using Api_test_ia.Application.UseCases.Auth.Commands;         // LoginCommand, RefreshTokenCommand, LogoutCommand
using Api_test_ia.Presentation.Contracts.Auth;
using Api_test_ia.Presentation.Mappings;

namespace Api_test_ia.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public sealed class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthTokens>> Login([FromBody] LoginRequest req, CancellationToken ct)
        {
            var (tokens, refresh) = await mediator.Send(req.ToCommand(), ct);

            Response.Cookies.Append("refresh_token", refresh, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(tokens);
        }

        // refresco vía cookie httpOnly (sin body)
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthTokens>> Refresh(CancellationToken ct)
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var refresh)) return Unauthorized();

            var (tokens, newRefresh) = await mediator.Send(new RefreshTokenCommand(refresh!), ct);

            Response.Cookies.Append("refresh_token", newRefresh, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(tokens);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var refresh))
                await mediator.Send(new LogoutCommand(refresh!), ct);

            Response.Cookies.Delete("refresh_token");
            return NoContent();
        }
    }
}
