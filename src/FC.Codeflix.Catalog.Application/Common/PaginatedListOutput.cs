namespace FC.Codeflix.Catalog.Application.Common;
public class PaginatedListOutput<TOutputItem>
{
    public int Page { get;  set; }
    public int PerPage { get;  set; }
    public long Total { get;  set; }
    public IReadOnlyList<TOutputItem> Items { get;  set; }
    public PaginatedListOutput(int page, int perPage, long total, IReadOnlyList<TOutputItem> items)
    {
        Page = page;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}
