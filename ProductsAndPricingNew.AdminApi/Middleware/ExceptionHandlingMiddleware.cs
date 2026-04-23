using Microsoft.AspNetCore.Diagnostics;

namespace ProductsAndPricingNew.AdminApi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}