using Microsoft.AspNetCore.OData.Query;
using Microsoft.FeatureManagement;
using Microsoft.OData;
using System.Diagnostics;

namespace ODataDemo.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal sealed class QueryOptionsValidator : EnableQueryAttribute
    {
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            var featureManager = request.HttpContext.RequestServices.GetService(typeof(IFeatureManager)) as IFeatureManager;
            Debug.Assert(featureManager != null);

            var filterQueryOptionRequired = featureManager.IsEnabledAsync(FeatureFlags.FilterQueryOptionRequired).GetAwaiter().GetResult();

            if (filterQueryOptionRequired && queryOptions.Filter == null)
                throw new ODataException("The $filter clause is required");

            base.ValidateQuery(request, queryOptions);
        }
    }
}
