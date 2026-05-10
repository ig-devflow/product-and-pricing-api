using FluentResults;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Assertions;

internal static class ResultAssertions
{
    public static TError AssertFailedWith<TError>(ResultBase result)
        where TError : IError
    {
        Assert.False(result.IsSuccess);
        return Assert.IsType<TError>(Assert.Single(result.Errors));
    }

    public static void AssertSucceeded(ResultBase result)
    {
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
    }
}
