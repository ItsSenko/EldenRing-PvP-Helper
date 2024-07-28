using Erd_Tools.Models;
using PvPHelper.MVVM.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Database.ItemsBases
{
    public class ItemTypeItems : IItemsBase<Item>
    {
        public string Name { get; set; }
        public List<Item> Items { get; set; }

        public Item.Category Category { get; set; }

        public ItemTypeItems(string name, List<Item> items, Item.Category category)
        {
            Name = name;
            Items = items;
            Category = category;
        }

        public void Update(object? sender, SearchableDataBase<Item> alg)
        {

            if (Name == "All")
            {
                alg.searchAlg.Items = alg.Items;
                return;
            }

            alg.searchAlg.Items = alg.Items.Where(x => x.ItemCategory == Category);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
