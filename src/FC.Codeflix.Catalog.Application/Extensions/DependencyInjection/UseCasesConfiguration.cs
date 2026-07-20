using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Application.Extensions.DependencyInjection;
public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCategory>());
        return services;
    }
}