using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Api.Configurations;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Api.Configurations;

public class UseCasesConfigurationTest
{
    [Fact(DisplayName = nameof(AddUseCases_Should_Register_Repositories_And_UnitOfWork))]
    [Trait("Integration/Api", "UseCasesConfiguration")]
    public void AddUseCases_Should_Register_Repositories_And_UnitOfWork()
    {
        var services = new ServiceCollection();

        services.AddDbContext<CodeflixCatalogDbContext>(options =>
            options.UseInMemoryDatabase($"use-cases-di-{Guid.NewGuid()}"));

        services.AddUseCases();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<ICategoryRepository>()
            .Should().BeOfType<CategoryRepository>();
        serviceProvider.GetRequiredService<IUnitOfWork>()
            .Should().BeOfType<UnitOfWork>();
    }
}