using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData;

namespace ODataDemo.Validators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal sealed class QueryOptionsValidator : EnableQueryAttribute
    {
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            if (queryOptions.Filter == null)
                throw new ODataException("The $filter clause is required");

            base.ValidateQuery(request, queryOptions);
        }
    }
}
