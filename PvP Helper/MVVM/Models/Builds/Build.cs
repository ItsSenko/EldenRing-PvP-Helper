using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Builds
{
    public class Build
    {
        public delegate void OnInventoryChangeHandler(Inventory inventory);
        public event OnInventoryChangeHandler OnInventoryChanged;

        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        private List<Inventory> _inventories;

        public List<Inventory> Inventories
        {
            get { return _inventories; }
            set 
            {
                if (_inventories != null)
                    foreach(var inventory in _inventories)
                        inventory.OnItemsChanged -= OnItemsChange;

                _inventories = value;

                if (_inventories != null)
                    foreach (var inventory in _inventories)
                        inventory.OnItemsChanged += OnItemsChange;
            }
        }

        public Build(string name, List<Inventory> inventories)
        {
            if (string.IsNullOrEmpty(name) || inventories == null)
                return;

            this.Name = name;
            this.Inventories = inventories;

            foreach (Inventory inventory in inventories)
                inventory.OnItemsChanged += (inv, items) => { OnInventoryChanged?.Invoke(inv); };
        }

        public override string ToString()
        {
            return Name;
        }

        private void OnItemsChange(Inventory inventory, List<BuildItem> items)
        {
            OnInventoryChanged?.Invoke(inventory);
        }
    }
}
