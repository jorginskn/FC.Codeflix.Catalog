
using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity.Category;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;
[Collection(nameof(GetCategoryTestFixtureCollection))]
public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;
    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetValidCategory();
        repositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);
        var input = new GetCategoryInput(exampleCategory.Id);
        var useCase = new UseCases.GetCategory(repositoryMock.Object);
        var output = await useCase.Handle(input, CancellationToken.None);
        output.Should().NotBeNull();
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Id.Should().NotBeEmpty();
        (output.CreatedAt != DateTime.MinValue).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(GetCategory_NotFound))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory_NotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();
        repositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));
        var input = new GetCategoryInput(exampleGuid);
        var useCase = new UseCases.GetCategory(repositoryMock.Object);
        var act = async () => await useCase.Handle(input, CancellationToken.None);
        act.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
