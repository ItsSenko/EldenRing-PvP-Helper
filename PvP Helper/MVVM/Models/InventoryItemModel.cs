using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PvPHelper.MVVM.Models
{
    public class InventoryItemModel : ViewModelBase
    {
        public ImageSource ItemIconPath { get; set; }
        public ImageSource AshOfWarIcon { get; set; }
        public ImageSource InfusionIconPath { get; set; }
        public string ItemName { get; set; }
        public string UpgradeLevel { get; set; }

        public InventoryItemModel(ImageSource itemIconPath, ImageSource ashOfWarIcon, ImageSource infusionIconPath, string itemName, string upgradeLevel = "")
        {
            ItemIconPath = itemIconPath;
            AshOfWarIcon = ashOfWarIcon;
            InfusionIconPath = infusionIconPath;
            ItemName = itemName;
            UpgradeLevel = upgradeLevel;
        }
    }
}
