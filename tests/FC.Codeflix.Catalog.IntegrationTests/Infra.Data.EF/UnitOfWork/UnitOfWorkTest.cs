using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkInfra = FC.Codeflix.Catalog.Infra.Data.EF;
namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork;
[Collection(nameof(UnitOfWorkTestFixtureCollection))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - UnitOfWork")]
    public async Task Commit()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoryList = _fixture.GetValidCategoryList();
        await dbContext.Categories.AddRangeAsync(exampleCategoryList);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
        await unitOfWork.Commit(CancellationToken.None);
        var assertDbContext = _fixture.CreateDbContext(true);
        var savedCategories = await assertDbContext.Categories.AsNoTracking().ToListAsync();
        savedCategories.Should().HaveCount(exampleCategoryList.Count);
    }


    [Fact(DisplayName = nameof(RollBack))]
    [Trait("Integration/Infra.Data", "UnitOfWork - UnitOfWork")]
    public async Task RollBack()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
        var task = async () => await unitOfWork.Rollback(CancellationToken.None);
        await task.Should().NotThrowAsync();

    }
}
