using System;
using System.Collections.Generic;
using System.Linq;

namespace CombinatorialEvolution.Sudoku
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            return items.OrderBy(_ => Guid.NewGuid());
        }
    }
}
