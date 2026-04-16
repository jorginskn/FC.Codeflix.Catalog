using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.SeedWork;

namespace FC.Codeflix.Catalog.Domain.Entity;

public class Category : AggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }


    public Category(string name, string description, bool isActive = true) : base()
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }

    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description ?? Description;
        Validate();
    }

    private void Validate()
    {
        if (String.IsNullOrWhiteSpace(Name))
        {
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
        }
        if (Name.Length < 3)
        {
            throw new EntityValidationException($"{nameof(Name)} should be at least 3 characters long");
        }
        if (Name.Length > 255)
        {
            throw new EntityValidationException($"{nameof(Name)} should be less or equal 255 characters long");
        }
        if (String.IsNullOrWhiteSpace(Description))
        {
            throw new EntityValidationException($"{nameof(Description)} should not be empty or null");
        }
        if (Description.Length > 10_000)
        {
            throw new EntityValidationException($"{nameof(Description)} should be less or equal 10_000 characters long");
        }
    }

}
