using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars.Utilities
{
    public static class EnumerableExtensions
    {
        private static Random random = new Random();

        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null || !enumerable.Any())
                throw new InvalidOperationException("Sequence is empty or null");

            int index = random.Next(enumerable.Count());
            return enumerable.ElementAt(index);
        }


        public static T PopRandomElement<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("List is empty or null");

            int index = new Random().Next(list.Count);
            T element = list[index];
            list.RemoveAt(index);
            return element;
        }
    }
}
