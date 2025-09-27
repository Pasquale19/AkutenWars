using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars.Utilities
{
    public static class ArrayExtensions
    {
        private static Random _random = new Random();

        public static T GetRandomElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
                throw new InvalidOperationException("Array is empty or null.");
            int index = _random.Next(array.Length);
            return array[index];
        }
    }
}
