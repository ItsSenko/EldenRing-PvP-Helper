using Erd_Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Erd_Tools.Models.Weapon;

namespace ERPvPHelper
{
    internal class ComboBoxOptions
    {
    }
    class ItemCategoryOption
    {
        public string Name { get; set; }
        public ItemCategory category { get; set; }
        public ItemCategoryOption(string name, ItemCategory category)
        {
            this.Name = name;
            this.category = category;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    class ItemGibOption
    {
        public string Name { get; set; }
        public Item item { get; set; }
        public ItemGibOption(string name, Item item)
        {
            this.Name = name;
            this.item = item;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    class InfusionOption
    {
        public string Name { get; set; }
        public Infusion infusion { get; set; }
        public InfusionOption(string name, Infusion item)
        {
            this.Name = name;
            this.infusion = item;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    class GemOption
    {
        public string Name { get; set; }
        public Gem gem { get; set; }
        public GemOption(string name, Gem item)
        {
            this.Name = name;
            this.gem = item;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    class ChrType
    {
        public int ChrID { get; set; }
        public int ParamID { get; set; }
        public string Name { get; set; }
        public Color diffMulColor { get; set; }
        public Color edgeColor { get; set; }
        public Color frontColor { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
