using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;
[Collection(nameof(CategoryRepositoryTestFixtureCollection))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;
    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Insert()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext(true);
        var exampleCategory = _fixture.GetValidCategory();
        var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Get()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext(true);
        var exampleCategory = _fixture.GetValidCategory();
        var exampleCategoriesList = _fixture.GetValidCategoryList();
        exampleCategoriesList.Add(exampleCategory);
        await dbContext.Categories.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
        var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        var exampleId = Guid.NewGuid();
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetValidCategoryList();
        await dbContext.Categories.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
        var task = async () => await categoryRepository.Get(exampleId, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{exampleId}' not found.");
    }


    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Update()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext(true);
        var exampleCategory = _fixture.GetValidCategory();
        var newCategoryValues = _fixture.GetValidCategory();
        await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);

        exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
        await categoryRepository.Update(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(SearchReturnListAndTotal))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchReturnListAndTotal()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetValidCategoryList();
        var newCategoryValues = _fixture.GetValidCategory();
        await dbContext.Categories.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.ASC);
        var output = await categoryRepository.Search(searchInput, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().HaveCount(exampleCategoriesList.Count);
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);

        foreach(Category ouputItem in output.Items)
        {
            var exampleCategory = exampleCategoriesList.FirstOrDefault(c => c.Id == ouputItem.Id);
            exampleCategory.Should().NotBeNull();
            ouputItem.Name.Should().Be(exampleCategory.Name);
            ouputItem.Description.Should().Be(exampleCategory.Description);
            ouputItem.IsActive.Should().Be(exampleCategory.IsActive);
            ouputItem.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Delete()
    {
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetValidCategory();
        await dbContext.Categories.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var categoryRepository = new Catalog.Infra.Data.EF.Repositories.CategoryRepository(dbContext);

        await categoryRepository.Delete(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var dbCategory = await dbContext.Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().BeNull();
    }
    
}
