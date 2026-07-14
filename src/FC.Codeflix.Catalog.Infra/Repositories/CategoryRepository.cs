using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.UnitTests.Application.ListCategories;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly CodeflixCatalogDbContext _context;
    private DbSet<Category> _categories => _context.Set<Category>();
    public CategoryRepository(CodeflixCatalogDbContext context)
    {
        _context = context;
    }
    public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _categories.AsNoTracking();
        if (!String.IsNullOrWhiteSpace(input.Search))
        {
            query = query.Where(c => c.Name.Contains(input.Search));
        }
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip(toSkip).Take(input.PerPage).ToListAsync(cancellationToken);
        return new (input.Page, input.PerPage, total, items);
    }

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var total = await _categories.CountAsync(cancellationToken);
        var category = await _categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (category is null)
            NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");
        return category!;
    }

    public Task<Category> Update(Category aggregate, CancellationToken cancellationToken)
    {
        _categories.Update(aggregate);
        return Task.FromResult(aggregate);
    }

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
    {       
        _categories.Remove(aggregate);
        return Task.CompletedTask;
    }



    public async Task Insert(Category aggregate, CancellationToken cancellationToken)
    {
        await _categories.AddAsync(aggregate, cancellationToken);
    }

  

}
