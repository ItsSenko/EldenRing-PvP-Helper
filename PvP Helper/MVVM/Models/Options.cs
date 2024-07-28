using Erd_Tools.Models;
using PvPHelper.MVVM.ViewModels;
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
    /*public class InventoryStateOption : BaseOption
    {
        public PrefabCreatorViewModel.InventoryState State { get; set; }
        public InventoryStateOption(PrefabCreatorViewModel.InventoryState state)
        {
            Name = state.ToString();
            State = state;
        }
    }*/

    public class MenuItem : BaseOption
    {
        public int Offset { get; set; }
        public int startId { get; set; }
        public int endId { get; set; }
        public MenuItem(string name, int offset, int startId = 0, int endId = 9999999)
        {
            this.Offset = offset;
            this.Name = name;
            this.startId = startId;
            this.endId = endId;
        }
    }
}
