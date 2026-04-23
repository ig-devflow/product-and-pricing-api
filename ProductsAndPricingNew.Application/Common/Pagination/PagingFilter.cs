namespace ProductsAndPricingNew.Application.Common.Pagination;

public readonly record struct PagingFilter
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 200;

    public PagingFilter(int? page, int? pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    private int? Page { get; init; }
    private int? PageSize { get; init; }

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