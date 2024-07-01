using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Search.SortOrders
{
    public class AlphabeticalSort<T> : ISortOrder<T>
    {
        public string Name { get => "Aplhabetical"; }

        public List<SearchItem<T>> Sort(List<SearchItem<T>> items, object? sender = null)
        {
            List<SearchItem<T>> result = items;
            result.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
            return result;
        }
    }
}
