using System;
using System.Collections.Generic;
using System.Linq;

namespace WaveSimulator.Extensions
{
    public static class ListExtensions
    {
        public static List<List<T>> GetAllCombos<T>(this ICollection<T> list)
        {
            List<List<T>> result = new List<List<T>>();
            // head
            result.Add(new List<T>());
            result.Last().Add(list.First());
            if (list.Count == 1)
                return result;
            // tail
            List<List<T>> tailCombos = GetAllCombos(list.Skip(1).ToArray());
            tailCombos.ForEach(combo =>
            {
                result.Add(new List<T>(combo));
                combo.Add(list.First());
                result.Add(new List<T>(combo));
            });
            return result;
        }


    }
}
