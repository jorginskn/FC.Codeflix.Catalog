namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public class CreateCategoryOutput
{ 
    public Guid Id { get;  set; }
    public string Name { get;  set; }
    public string? Description { get;  set; }
    public bool IsActive { get;  set; }
    public DateTime CreatedAt { get;  set; }
    public CreateCategoryOutput(
       Guid Id,
       string name,
       string? description,
       bool isActive,
       DateTime createdAt)
    {
        this.Id = Id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
    }
}
