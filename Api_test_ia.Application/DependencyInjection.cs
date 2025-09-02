using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Api_test_ia.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 1) Escanea el ensamblado de Application para registrar Handlers de MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));

            // 2) Escanea el mismo ensamblado para registrar Validators de FluentValidation
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // 3) (Opcional) Registrar behaviors, decoradores, etc.
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
