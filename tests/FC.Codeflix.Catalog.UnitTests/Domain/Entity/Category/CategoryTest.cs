using Xunit;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;
[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _fixture;

    public CategoryTest(CategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _fixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var dateTimeAfter = DateTime.Now;
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActiveStatus))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActiveStatus(bool isActive)
    {
       var validCategory = _fixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var dateTimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _fixture.GetValidCategory();
        Action action = () => new DomainEntity.Category(name!, validCategory.Description);
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    public void InstantiateErrorWhenDescriptionIsNull(string? description)
    {
        var validCategory = _fixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(validCategory.Name, description);
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be empty or null");
    }


    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characteres))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("ab")]
    [InlineData("a")]
    public void InstantiateErrorWhenNameIsLessThan3Characteres(string invalidName)
    {
        var validCategory = _fixture.GetValidCategory();

        Action action = () => new DomainEntity.Category(invalidName!, validCategory.Description);
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characteres))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characteres()
    {
        var validCategory = _fixture.GetValidCategory();

        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characteres))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characteres()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10_000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    { 
        var validCategory = _fixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();
        (category.IsActive).Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        var validCategory = _fixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Deactivate();
        (category.IsActive).Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var validCategory = _fixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        var categoryWithNewValues = _fixture.GetValidCategory();
 
        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);
        category.Name.Should().Be(categoryWithNewValues.Name);
        category.Description.Should().Be(categoryWithNewValues.Description);

    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var validCategory = _fixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        var newValues = _fixture.GetValidCategory();
        var currentDescription = category.Description;
        category.Update(newValues.Name);
        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(currentDescription);
    }


    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsNull(string? name)
    {
        var validCategory = _fixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        Action action = () => category.Update(name!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characteres))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("ab")]
    [InlineData("a")]
    public void UpdateErrorWhenNameIsLessThan3Characteres(string invalidName)
    {
        var validCategory = _fixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        Action action = () => category.Update(invalidName!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characteres))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characteres()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        Action action = () => category.Update(invalidName!);
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }


    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characteres))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characteres()
    {
        var validCategory = _fixture.GetValidCategory();
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        Action action = () => category.Update("Category New Name", invalidDescription);
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10_000 characters long");
    }

}
