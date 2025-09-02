using Microsoft.AspNetCore.Mvc;

namespace Api_test_ia.Presentation.Middlewares
{
    public sealed class ExceptionMappingMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext ctx)
        {
            try { await next(ctx); }
            catch (FluentValidation.ValidationException ex)
            {
                var pd = new ValidationProblemDetails(ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                { Status = StatusCodes.Status400BadRequest, Title = "Validation failed" };
                ctx.Response.StatusCode = pd.Status!.Value;
                await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (KeyNotFoundException ex)
            {
                var pd = new ProblemDetails { Status = 404, Title = "Not Found", Detail = ex.Message };
                ctx.Response.StatusCode = 404; await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (InvalidOperationException ex)
            {
                var pd = new ProblemDetails { Status = 409, Title = "Conflict", Detail = ex.Message };
                ctx.Response.StatusCode = 409; await ctx.Response.WriteAsJsonAsync(pd);
            }
            catch (UnauthorizedAccessException)
            {
                var pd = new ProblemDetails { Status = 401, Title = "Unauthorized" };
                ctx.Response.StatusCode = 401; await ctx.Response.WriteAsJsonAsync(pd);
            }
        }
    }
}
