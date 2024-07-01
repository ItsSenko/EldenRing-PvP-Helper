using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Search
{
    public class SearchItem<T>
    {
        public T Item { get; private set; }
        private string _name;
        public SearchItem(T item, string name)
        {
            Item = item;
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
