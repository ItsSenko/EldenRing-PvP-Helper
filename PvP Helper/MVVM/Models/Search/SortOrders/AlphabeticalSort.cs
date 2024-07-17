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

        public List<T> Sort(IEnumerable<T> items, object? sender = null)
        {
            List<T> result = items.ToList();
            result.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
