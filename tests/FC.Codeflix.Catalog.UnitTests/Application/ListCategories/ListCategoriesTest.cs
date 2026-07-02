using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategories;
[Collection(nameof(ListCategoriesTestFixtureCollection))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;
    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListCategories))]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task ListCategories()
    {
       var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetExampleInput();
        var outputRepositorySearch = new SearchOutput<Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: (IReadOnlyList<Category>)_fixture.GetValidCategoriesList(15),
            total: 70
        );
        repositoryMock.Setup(x => x.Search(It.Is<SearchInput>(searchInput => searchInput.Page == input.Page 
            && searchInput.PerPage == input.PerPage 
            && searchInput.Search == input.Search 
            && searchInput.OrderBy == input.Sort 
            && searchInput.Order == input.Dir), It.IsAny<CancellationToken>()))
            .ReturnsAsync(outputRepositorySearch);
        var useCase = new UseCases.ListCategories(repositoryMock.Object);
        var output = await useCase.Handle(input, CancellationToken.None);
        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        output.Items.ToList().ForEach(outputItem =>
        {
            var repositoryItem = outputRepositorySearch.Items.FirstOrDefault(x => x.Id == outputItem.Id);
            repositoryItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryItem!.Name);
            outputItem.Description.Should().Be(repositoryItem.Description);
            outputItem.IsActive.Should().Be(repositoryItem.IsActive);
            outputItem.CreatedAt.Should().Be(repositoryItem.CreatedAt);
        });
        repositoryMock.Verify(x => x.Search(It.Is<SearchInput>(searchInput => searchInput.Page == input.Page 
            && searchInput.PerPage == input.PerPage 
            && searchInput.Search == input.Search 
            && searchInput.OrderBy == input.Sort 
            && searchInput.Order == input.Dir), It.IsAny<CancellationToken>()), Times.Once);
    }

  
}
