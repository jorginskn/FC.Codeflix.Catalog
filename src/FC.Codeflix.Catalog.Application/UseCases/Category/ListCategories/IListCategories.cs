using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
public interface IListCategories : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{
    public Task<ListCategoriesOutput> Handle(ListCategoriesInput input, CancellationToken cancellationToken);
}
