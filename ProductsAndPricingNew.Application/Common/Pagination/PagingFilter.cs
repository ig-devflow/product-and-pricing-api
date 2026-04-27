namespace ProductsAndPricingNew.Application.Common.Pagination;

public readonly record struct PagingFilter
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 200;

    private int? Page { get; }
    private int? PageSize { get; }

    public PagingFilter(int? page, int? pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public int GetPage()
    {
        return Page.HasValue && Page.Value > 0
            ? Page.Value
            : DefaultPage;
    }

    public int GetPageSize()
    {
        if (!PageSize.HasValue || PageSize.Value <= 0)
            return DefaultPageSize;

        return PageSize.Value > MaxPageSize
            ? MaxPageSize
            : PageSize.Value;
    }
}