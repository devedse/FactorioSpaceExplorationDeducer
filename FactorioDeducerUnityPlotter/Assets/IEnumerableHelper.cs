using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IEnumerableHelper
{
    public static IEnumerable<T> GetElementsAt<T>(this IEnumerable<T> data, IEnumerable<int> itemsToSelect)
    {
        foreach (var itemToSelect in itemsToSelect)
        {
            yield return data.ElementAt(itemToSelect);
        }
    }
}
