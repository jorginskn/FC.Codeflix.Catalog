using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;
[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName =nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task UpdateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryExample = _fixture.GetValidCategory();
        repositoryMock.Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);
        var input = new UpdateCategoryInput(categoryExample.Id, _fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription(), _fixture.GetRandomBoolean());
        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
       CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);
        output.Should().NotBeNull();
        output.Name.Should().Be(categoryExample.Name);
        output.Description.Should().Be(categoryExample.Description);
        output.IsActive.Should().Be(categoryExample.IsActive);
        repositoryMock.Verify(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}
