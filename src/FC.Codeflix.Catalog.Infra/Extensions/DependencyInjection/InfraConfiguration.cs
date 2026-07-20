using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Extensions.DependencyInjection;
public static class InfraConfiguration
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
