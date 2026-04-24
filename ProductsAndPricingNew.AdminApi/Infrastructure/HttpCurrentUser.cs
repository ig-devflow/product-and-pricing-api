using ProductsAndPricingNew.Domain.Abstractions;

namespace ProductsAndPricingNew.AdminApi.Infrastructure;

public sealed class HttpCurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            string? raw = _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value
                          ?? _httpContextAccessor.HttpContext?.User.FindFirst("userid")?.Value;

            return int.TryParse(raw, out int userId)
                ? userId
                : 0;
        }
    }
}