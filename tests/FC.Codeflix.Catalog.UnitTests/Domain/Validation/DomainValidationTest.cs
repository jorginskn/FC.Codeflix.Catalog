using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;
public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
        Action action = () => DomainValidation.NotNull(value, "value");
        action.Should().NotThrow();
    }
    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string value = null;
        Action action = () => DomainValidation.NotNull(value, "fieldName");
        action.Should().Throw<EntityValidationException>()
            .WithMessage("The fieldName should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData(null)]
    [InlineData("")]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");
        action.Should().Throw<EntityValidationException>()
             .WithMessage("The fieldName should not be null or empty");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]

    public void NotNullOrEmptyOk()
    {
        string target = Faker.Commerce.ProductName();
        Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLenghtThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("aa", 3)]
    public void MinLenghtThrowWhenLess(string target, int minLenght)
    {
        Action action = () => DomainValidation.MinLenght(target, minLenght, "fieldName");
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"The fieldName should not be less than {minLenght} characters");
    }
    
    [Fact(DisplayName = nameof(MinLenghtOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void MinLenghtOk()
    {
        string target = Faker.Commerce.ProductName();
        Action action = () => DomainValidation.MinLenght(target, 3, "fieldName");
        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MaxLenghtThrowWhenGreater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetGreaterThanMax), parameters: 10)]
    public void MaxLenghtThrowWhenGreater(string target, int maxLenght)
    {
        Action action = () => DomainValidation.MaxLenght(target, maxLenght, "fieldName");
        action.Should().Throw<EntityValidationException>()
            .WithMessage($"The fieldName should not be greater than {maxLenght} characters");
    }
     
    public static IEnumerable<object[]> GetGreaterThanMax(int numberOftests)
    {
        yield return new object[] { "123456", 5 };
        var faker = new Faker();
        for (int i = 0; i < numberOftests; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLenght = example.Length - (new Random().Next(1, 5));
        }
    }
     
}
