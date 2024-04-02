using Microsoft.Extensions.Primitives;

namespace ODataDemo.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool ShouldOmitNullValues(this HttpRequest request)
        {
            string preferHeader = null;
            StringValues values;
            if (request.Headers.TryGetValue(PreferHeaderName, out values))
            {
                // If there are many "Prefer" headers, pick up the first one.
                preferHeader = values.FirstOrDefault();
            }

            // use case insensitive string comparison for simplicity
            if (preferHeader != null && preferHeader.Contains(OmitNullValuesHeaderValue, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public const string PreferHeaderName = "Prefer";
        public const string OmitNullValuesHeaderValue = "omit-values=nulls";
    }
}
