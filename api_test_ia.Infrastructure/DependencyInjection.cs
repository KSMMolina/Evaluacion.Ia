using Api_test_ia.Application.Abstractions.Persistence;
using Api_test_ia.Application.Abstractions.Persistence.Auth;
using Api_test_ia.Application.Abstractions.Persistence.Categories;
using Api_test_ia.Application.Abstractions.Persistence.Products;
using Api_test_ia.Application.Abstractions.Persistence.Users;
using Api_test_ia.Application.Abstractions.Security;
using Api_test_ia.Application.Abstractions.Storage;
using Api_test_ia.Infrastructure.Auth;                 // JwtOptions
using Api_test_ia.Infrastructure.Persistence.Context;
using Api_test_ia.Infrastructure.Persistence.Uow;
using Api_test_ia.Infrastructure.Repositories.Categories;
using Api_test_ia.Infrastructure.Repositories.Products;
using Api_test_ia.Infrastructure.Repositories.Users;
using Api_test_ia.Infrastructure.Security;            // JwtProvider, BcryptPasswordHasher
using Api_test_ia.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_test_ia.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            // JWT config
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            var jwt = configuration.GetSection("Jwt").Get<JwtOptions>()!;

            // AuthN/AuthZ
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = JwtRegisteredClaimNames.Email
                    };
                });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("AdminOrEditor", p => p.RequireRole("Admin", "Editor"));
                o.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
            });

            // Puertos -> Adaptadores
            services.AddScoped<IUserRepository, EfUserRepository>();
            services.AddScoped<IRefreshTokenStore, EfRefreshTokenStore>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            services.AddScoped<IProductQueries, EfProductQueries>();
            services.AddScoped<IProductCommands, EfProductCommands>();
            services.AddScoped<ICategoryQueries, EfCategoryQueries>();
            services.AddScoped<ICategoryCommands, EfCategoryCommands>();
            services.AddScoped<IUserQueries, EfUserQueries>();
            services.AddScoped<IUserCommands, EfUserCommands>();

            services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));
            services.AddSingleton<IImageStorage, FileSystemImageStorage>();

            return services;
        }
    }
}
