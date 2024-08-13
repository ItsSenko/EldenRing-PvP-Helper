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
    public class MenuItem
    {
        public string Name { get; set; }
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

        public override string ToString()
        {
            return Name;
        }
    }
}
