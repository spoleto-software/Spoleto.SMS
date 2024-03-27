namespace Spoleto.SMS.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Enumerable"/> type.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs the specified action on each element of the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to iterate over.</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each element of the <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="action"/> is null.</exception>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null) 
                throw new ArgumentNullException(nameof(action));

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

        /// <summary>
        /// Gets the data with the given name.
        /// </summary>
        /// <param name="data">The source data list.</param>
        /// <param name="key">The data name</param>
        /// <returns>The <see cref="SmsProviderData"/> instance</returns>
        public static SmsProviderData GetData(this IEnumerable<SmsProviderData> data, string name)
            => data.FirstOrDefault(e => e.Name == name);
    }
}
