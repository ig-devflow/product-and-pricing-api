using System.Reflection;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.ArchitectureTests;

public sealed class LayerDependencyTests
{
    [Fact]
    public void Domain_DoesNotReferenceOuterLayersOrInfrastructure()
    {
        Assembly domainAssembly = typeof(Division).Assembly;

        AssertDoesNotReference(domainAssembly,
        [
            "ProductsAndPricingNew.Application",
            "ProductsAndPricingNew.Persistence",
            "ProductsAndPricingNew.AdminApi",
            "Microsoft.EntityFrameworkCore",
            "FluentValidation",
            "MediatR",
            "Dapper",
            "Microsoft.AspNetCore",
            "Microsoft.AspNetCore.Mvc"
        ]);
    }

    [Fact]
    public void Application_DoesNotReferencePersistenceOrAdminApi()
    {
        Assembly applicationAssembly = typeof(Application.ServiceCollectionExtensions).Assembly;

        AssertDoesNotReference(applicationAssembly,
        [
            "ProductsAndPricingNew.Persistence",
            "ProductsAndPricingNew.AdminApi"
        ]);
    }

    [Fact]
    public void Persistence_DoesNotReferenceAdminApi()
    {
        Assembly persistenceAssembly = typeof(Persistence.ServiceCollectionExtensions).Assembly;

        AssertDoesNotReference(persistenceAssembly, ["ProductsAndPricingNew.AdminApi"]);
    }

    private static void AssertDoesNotReference(Assembly assembly, IReadOnlyCollection<string> forbiddenAssemblyNames)
    {
        HashSet<string> referencedAssemblyNames = assembly
            .GetReferencedAssemblies()
            .Select(reference => reference.Name!)
            .ToHashSet(StringComparer.Ordinal);

        foreach (string forbiddenAssemblyName in forbiddenAssemblyNames)
        {
            Assert.DoesNotContain(
                referencedAssemblyNames,
                referencedAssemblyName => referencedAssemblyName.StartsWith(forbiddenAssemblyName, StringComparison.Ordinal));
        }
    }
}
