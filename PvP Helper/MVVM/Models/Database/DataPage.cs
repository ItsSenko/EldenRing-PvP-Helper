using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Database
{
    public class DataPage<T>
    {
        public delegate void OnItemsChangedHandler(List<T> items);
        public event OnItemsChangedHandler OnItemsChanged;

        private List<T> _items;

        public List<T> Items
        {
            get { return _items; }
            set { _items = value; OnItemsChanged?.Invoke(value); }
        }

        public DataPage(List<T> items)
        {
            Items = items;
        }
    }
}
