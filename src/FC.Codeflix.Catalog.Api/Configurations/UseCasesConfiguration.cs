using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using MediatR;

namespace FC.Codeflix.Catalog.Api.Configurations;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateCategory));
        return services;
    }
}
