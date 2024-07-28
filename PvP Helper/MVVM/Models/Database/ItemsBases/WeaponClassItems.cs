using Erd_Tools.Models;
using PvPHelper.MVVM.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Database.ItemsBases
{
    public class WeaponClassItems : IItemsBase<Item>
    {
        public string Name { get; set; }
        public List<Item> Items { get; set; }
        public Weapon.WeaponType Type { get; set; }

        public WeaponClassItems(string name, List<Item> items, Weapon.WeaponType type)
        {
            Name = name;
            Items = items;
            Type = type;
        }

        public void Update(object? sender, SearchableDataBase<Item> alg)
        {

            List<Item> FinalList = new();

            foreach(Item item in alg.Items)
            {
                if (item is Weapon weapon)
                {
                    if (weapon.Type == Type)
                        FinalList.Add(item);
                    else if (Name == "All")
                        FinalList.Add(item);
                }
            }

            alg.searchAlg.Items = FinalList;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
