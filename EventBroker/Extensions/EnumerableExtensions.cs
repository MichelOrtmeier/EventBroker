namespace EventBroker.Extensions
{
    internal static class EnumerableExtensions
    {
        public static bool ContentEqual<T>(this IEnumerable<T> values, IEnumerable<T> comparedValues)
        {
            IEnumerable<T> orderedValues = values.OrderByHashCode();
            IEnumerable<T> comparedOrderedValues = comparedValues.OrderByHashCode();
            return orderedValues.SequenceEqual(comparedOrderedValues);
        }

        private static IEnumerable<T> OrderByHashCode<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy((obj) => obj?.GetHashCode() ?? 0);
        }
    }
}
