using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static void Apply<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T value) => enumerable.Concat(value.Yield());

        public static IEnumerable<T> ConcatFirst<T>(this IEnumerable<T> enumerable, T value) => value.Yield().Concat(enumerable);

        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }

        public static T[] ToArrayOrNull<T>(this IEnumerable<T> enumerable)
        {
            var result = enumerable != null ? enumerable.ToArray() : Array.Empty<T>();
            return result.Length == 0 ? null : result;
        }

        public static Task<TResult[]> SelectAsync<T, TResult>(this IEnumerable<T> source, Func<T, Task<TResult>> asyncSelector) => Task.WhenAll(source.Select(asyncSelector));

        public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> asyncSelector) => Task.WhenAll(source.Select(asyncSelector));

        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return !source.Any() ? source :
                source.Concat(
                    source
                    .SelectMany(i => selector(i).EmptyIfNull())
                    .SelectManyRecursive(selector)
                );
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();

    }
}
