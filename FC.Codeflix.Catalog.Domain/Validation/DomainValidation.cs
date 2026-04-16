namespace FC.Codeflix.Catalog.Domain.Validation;
public class DomainValidation
{
    public static void NotNull(object target, string fieldName)
    {
        if (target is null)
        {
            throw new Exceptions.EntityValidationException($"The {fieldName} should not be null");
        }
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            throw new Exceptions.EntityValidationException($"The {fieldName} should not be null or empty");
        }
    }

    public static void MinLenght(string target, int minLenght, string fieldName)
    {
        if (target.Length < minLenght)
        {
            throw new Exceptions.EntityValidationException($"The {fieldName} should not be less than {minLenght} characters");
        }
    }

    public static void MaxLenght(string target, int maxLenght, string fieldName)
    {
        if (target.Length > maxLenght)
        {
            throw new Exceptions.EntityValidationException($"The {fieldName} should not be greater than {maxLenght} characters");
        }
    }

}
