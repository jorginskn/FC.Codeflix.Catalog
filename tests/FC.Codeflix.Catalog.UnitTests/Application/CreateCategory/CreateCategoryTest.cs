using Bogus.DataSets;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using Moq;
using Xunit;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;
public class CreateCategoryTest
{
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async Task CreateCategory()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWork.Object);
        var input = new UseCases.CreateCategoryInput(
            "category name",
            "category description",
            true
        );
        var output = await useCase.Handle(input);

        repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once());
        unitOfWork.Verify(unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()), Times.Once());
        output.Should().NotBeNull();
        output.Name.Should.Be("category name");
        output.Description.Should.Be("category description");
        output.IsActive.Should.Be(true);
        (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != null && output.CreatedAt != DateTime.MinValue).Should().BeTrue(); 
 
    }
}
