using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;
[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture) => _fixture = fixture;

    [Theory(DisplayName =nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))] 
    public async Task UpdateCategory(Category categoryExample, UpdateCategoryInput input)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);
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
    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryOnlyName(Category categoryExample, UpdateCategoryInput exampleInput)
    {
        var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name);
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);
        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);
        output.Name.Should().Be(categoryExample.Name);
        output.Description.Should().Be(categoryExample.Description);
        output.IsActive.Should().Be(categoryExample.IsActive);
        repositoryMock.Verify(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryWithoutProvidingIsActive(Category categoryExample, UpdateCategoryInput exampleInput)
    {
        var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);
        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);
        output.Name.Should().Be(categoryExample.Name);
        output.Description.Should().Be(categoryExample.Description);
        output.IsActive.Should().Be(categoryExample.IsActive);
        repositoryMock.Verify(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleGuidId =  Guid.NewGuid();
        repositoryMock.Setup(x => x.Get(exampleGuidId, It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"category '{exampleGuidId}' not found"));
        var input = new UpdateCategoryInput(exampleGuidId, _fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription(), _fixture.GetRandomBoolean());
        var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        var task = async () => await useCase.Handle(input, CancellationToken.None);
        await task.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.Get(exampleGuidId, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }
}
