namespace Spoleto.SMS.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Enumerable"/> type.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Foreach by any IEnumerable source.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                return;
            }

            var enumerator = source.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
            finally
            {
                enumerator?.Dispose();
            }
        }
    }
}
