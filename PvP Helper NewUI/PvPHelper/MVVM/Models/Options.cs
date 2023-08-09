using Erd_Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Erd_Tools.Models.Weapon;

namespace PvPHelper.MVVM.Models
{
    class ItemCategoryOption : BaseOption
    {
        public ItemCategory category { get; set; }
        public ItemCategoryOption(string name, ItemCategory category)
        {
            this.Name = name;
            this.category = category;
        }
    }
    class ItemGibOption : BaseOption
    {
        public string CatName { get; set; }
        public Item item { get; set; }
        public ItemGibOption(string name, string catName, Item item)
        {
            this.Name = name;
            this.item = item;
            CatName = catName;
        }
    }
    class InfusionOption : BaseOption
    {
        public Infusion infusion { get; set; }
        public InfusionOption(string name, Infusion item)
        {
            this.Name = name;
            this.infusion = item;
        }
    }
    class GemOption : BaseOption
    {
        public Gem gem { get; set; }
        public GemOption(string name, Gem item)
        {
            this.Name = name;
            this.gem = item;
        }
    }
}
