using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory;
[Collection(nameof(GetCategoryTestFixtureCollection))]
public class GetCategoryInputValidationTest
{
    private readonly GetCategoryTestFixture _fixture;
    public GetCategoryInputValidationTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetCategory - Use Cases")]
    public void ValidationOk()
    {
        var validInput = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidator();
        var ValidationResult = validator.Validate(validInput);
        ValidationResult.Should().NotBeNull();
        ValidationResult.IsValid.Should().BeTrue();
        ValidationResult.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "GetCategory - Use Cases")]
    public void InvalidWhenEmptyGuidId()
    {
        var invalidInput = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidator();
        var ValidationResult = validator.Validate(invalidInput);
        ValidationResult.Should().NotBeNull();
        ValidationResult.IsValid.Should().BeFalse();
        ValidationResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}
