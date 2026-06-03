using FC.Codeflix.Catalog.UnitTests.Common;
using Bogus;
using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture>
{

}

public class CreateCategoryTestFixture : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = new Faker().Commerce.ProductName();
        if (categoryName.Length > 255)
        {
            return categoryName[..255];
        }
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = new Faker().Commerce.ProductDescription();
        if (categoryDescription.Length > 10_000)
        {
            return categoryDescription[..10_000];
        }
        return categoryDescription;
    }

    public bool GetRandomBoolean()
    {
        return (new Random().NextDouble() < 0.5);
    }

    public CreateCategoryInput GetValidInput() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );

    public Mock<ICategoryRepository> GetRepositoryMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
}
