using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Builds
{
    public class Inventory
    {
        public delegate void OnItemsChangeHandler(Inventory inventory, List<BuildItem> items);
        public event OnItemsChangeHandler OnItemsChanged;

        public string Name { get; set; }

        private List<BuildItem> _items;
        public List<BuildItem> Items
        {
            get { return _items; }
            set { _items = value; OnItemsChanged?.Invoke(this, _items); }
        }

        public Inventory(string name, List<BuildItem> items)
        {
            Name = name;
            Items = items;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
