using Api_test_ia.Presentation.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Api_test_ia.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // ProblemDetails (.NET 8)
            services.AddProblemDetails();

            // CORS (lee origins del appsettings)
            var allowed = configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
            services.AddCors(o => o.AddPolicy("Front", p =>
                p.WithOrigins(allowed).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

            // Swagger + Bearer
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api_test_ia", Version = "v1" });

                var jwtScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Pega aquí tu access token **sin** la palabra 'Bearer'."
                };
                c.AddSecurityDefinition("Bearer", jwtScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, Array.Empty<string>() }
            });
            });

            return services;
        }

        public static WebApplication UsePresentation(this WebApplication app)
        {
            // Errores como ProblemDetails
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }

            // Swagger solo en dev (ajústalo si quieres siempre)
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("Front");

            app.UseMiddleware<ExceptionMappingMiddleware>();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
