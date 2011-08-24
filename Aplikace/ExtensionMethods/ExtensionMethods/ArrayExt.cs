using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class ArrayExt
    {
        private static Random rand = new Random();

        public static bool Find<T>(this T[] array, T item)
        {
            return Array.IndexOf(array, item) >= 0;
        }

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (T item in array)
            {
                action(item);
            }
        }

        public static T[][] Split<T>(this T[] array, int count)
        {
            int maximumItems = (int) Math.Ceiling((double)array.Length / (double)count);

            T[][] splitedArray = new T[count][];
            int index;

            for (int i = 0; i < count; i++)
            {
                splitedArray[i] = new T[maximumItems];
                for (int j = 0; j < maximumItems; j++)
                {
                    index = i * maximumItems + j;
                    if (index < array.Length)
                        splitedArray[i][j] = array[index];
                    else
                        break;
                }
            }

            return splitedArray;
        }

        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            return source.Select(t => new { Index = rand.Next(), Value = t }).OrderBy(p => p.Index).Select(p => p.Value);
        }

        /// <summary>
        /// Naháže obsah seznamu do jednoho řetězce, jako oddělovač použije Delimiter.
        /// </summary>
        /// <typeparam name="T">Datový typ seznamu</typeparam>
        /// <param name="List"></param>
        /// <param name="Delimiter">Řetězec, kterým budou odděleny jednotlivé položky seznamu</param>
        /// <returns></returns>
        public static string Implode<T>(this IEnumerable<T> List, string Delimiter)
        {
            StringBuilder NewString = new StringBuilder();

            foreach (T item in List)
                NewString.AppendFormat("{0}{1}", item.ToString(), Delimiter);

            return NewString.ToString();
        }

        public static string Implode<T>(this IEnumerable<T> List)
        {
            return List.Implode("");
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource item)
        {
            int index = 0;

            foreach (TSource sourceItem in source)
            {
                if (sourceItem.Equals(item))
                    return index;

                index++;
            }

            return -1;
        }

        public static TSource[] Take<TSource>(this TSource[] source, int from, int count)
        {
            int itemsCount = Math.Min(count, source.Length - from);
            TSource[] arrayPart = new TSource[itemsCount];
            int index = 0;
            for (int i = from; i < itemsCount + from; i++)
                arrayPart[index++] = source[i];

            return arrayPart;
        }

        /// <summary>
        /// Naplní celé pole předanou hodnotou
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T[] Fill<T>(this T[] arr, T value)
        {
            arr.Length.Times(x => arr[x] = value);
            return arr;
        }

        public static T[] Fill<T>(this T[] arr, Func<int, T> aggregate)
        {
            arr.Length.Times(x => arr[x] = aggregate(x));
            return arr;
        }
    }
}
