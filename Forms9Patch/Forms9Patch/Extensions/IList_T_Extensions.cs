using System;
using System.Collections.Generic;

namespace Forms9Patch
{
    internal static class IList_T_Extensions
    {
        public static IList<T> GetRange<T>(this IList<T> list, int startIndex, int count)
        {
            var result = new List<T>();
            var index = 0;
            var itemsCollected = 0;
            foreach (var item in list)
            {
                if (index >= startIndex)
                {
                    result.Add(item);
                    itemsCollected++;
                    if (itemsCollected >= count)
                        break;
                }
            }
            return result;
        }

        public static void RemoveRange<T>(this IList<T> list, int startIndex, int count)
        {
            for(int i=0; i<count;i++)
                list.RemoveAt(startIndex);
        }

        public static void InsertRange<T>(this IList<T> list, int startIndex, IList<T> range)
        {
            if (range == null || range.Count < 1)
                return;
            for (int i = range.Count - 1; i <= 0; i++)
                list.Insert(startIndex, range[i]);
        }


    }
}