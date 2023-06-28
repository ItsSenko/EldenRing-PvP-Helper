using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Erd_Tools;
using Erd_Tools.Models;

namespace ERPvPHelper
{
    public class Blacklist
    {
        public static List<BlacklistItem> blacklistedItems = new();

        public static void Initialize()
        {
            blacklistedItems = loadBlacklistedItems();
        }

        public static List<BlacklistItem> loadBlacklistedItems()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Resources.Blacklist.json";

            var items = JsonConvert.DeserializeObject<List<BlacklistItem>>(Helpers.GetEmbededResource(resourceName));
            if (items != null && items.Count > 0)
                return items;

            return new();
        }
    }
    public class BlacklistItem
    {
        public string CatName { get; set; }
        public int ItemID { get; set; }

        public BlacklistItem(string catName, int itemID)
        {
            CatName = catName;
            ItemID = itemID;
        }

    }
}
