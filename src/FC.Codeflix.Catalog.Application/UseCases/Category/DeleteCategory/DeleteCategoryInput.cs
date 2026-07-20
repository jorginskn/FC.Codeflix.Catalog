using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
public class DeleteCategoryInput : IRequest
{
    public Guid Id { get; }

    public DeleteCategoryInput(Guid id) => Id = id;
}