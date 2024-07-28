using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Search
{
    public interface ISortOrder<T>
    {
        public string Name { get; }

        public abstract List<T> Sort(IEnumerable<T> items, object? sender = null);
    }
}
