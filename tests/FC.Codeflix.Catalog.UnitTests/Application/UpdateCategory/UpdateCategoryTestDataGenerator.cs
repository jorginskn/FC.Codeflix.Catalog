using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;
public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]>GetCategoriesToUpdate(int numberOfCategories = 10)
    {
        var fixture = new UpdateCategoryTestFixture();
        for (int i = 0; i < numberOfCategories; i++)
        {
            var categoryExample = fixture.GetValidCategory();
            var input = new UpdateCategoryInput(categoryExample.Id, fixture.GetValidCategoryName(), fixture.GetValidCategoryDescription(), fixture.GetRandomBoolean());
            yield return new object[] { categoryExample, input };
        }
    }
}
