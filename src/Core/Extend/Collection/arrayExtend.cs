using System;
using System.Collections.Generic;
using System.Linq;
namespace System.Collections.Generic
{
    public static class arrayExtend
    {
        public static void AddLimit<T>(this List<T> list, T input, int number = 100)
        {
            list.Add(input);
            if (list.Count > 100)
            {
                var deleteTo = list.Count - number;
                if (deleteTo <= 0)
                    return;
                try
                {
                    list.RemoveRange(0, deleteTo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static IEnumerable<T> GetIEnumerableByIndex<T>(this List<T> list, int number = 100, int tryLimit = -1)
        {
            int count = 0;
            start:
            try
            {
                return list.Take(number).ToList();
            }
            catch
            {
                if (tryLimit < 0)
                    goto start;
                count++;
                if (count > tryLimit)
                    return Enumerable.Empty<T>();
                goto start;
            }
        }
    }
}
