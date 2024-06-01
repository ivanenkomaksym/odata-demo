namespace ODataDemo.Extensions
{
    public static class StringConverters
    {
        public static string ToCamelCase(this string someString)
        {
            return System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(someString);
        }
    }
}
