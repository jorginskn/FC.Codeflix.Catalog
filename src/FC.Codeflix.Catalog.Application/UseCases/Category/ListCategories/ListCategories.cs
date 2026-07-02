using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repository;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
public class ListCategories : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;
    public ListCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<ListCategoriesOutput> Handle(ListCategoriesInput input, CancellationToken cancellationToken)
    {
        var serachOutput = await _categoryRepository.Search(
           new(
            input.Page,
            input.PerPage,
            input.Search,
            input.Sort,
            input.Dir
             ), cancellationToken
        );
        var output =new ListCategoriesOutput(
             serachOutput.CurrentPage,
             serachOutput.PerPage,
             serachOutput.Total,
             serachOutput.Items.Select(x => CategoryModelOutput.FromCategory(x)).ToList()
        );
        return output;
    }
}
