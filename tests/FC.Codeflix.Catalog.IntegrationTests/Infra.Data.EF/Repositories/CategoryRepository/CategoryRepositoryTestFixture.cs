using FC.Codeflix.Catalog.IntegrationTests.Base;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;
[CollectionDefinition(nameof(CategoryRepositoryTestFixtureCollection))]
public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture> { }
 
public class CategoryRepositoryTestFixture : BaseFixture
{
}
