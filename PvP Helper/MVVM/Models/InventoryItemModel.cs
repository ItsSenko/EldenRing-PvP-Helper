using Erd_Tools.Models;
using PvPHelper.Core;
using PvPHelper.Core.Extensions;
using PvPHelper.MVVM.Models.Builds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PvPHelper.MVVM.Models
{
    public class InventoryItemModel : ViewModelBase
    {
        public delegate void OnClickHandler(BuildItem item);
        public event OnClickHandler OnLeftClick;
        public event OnClickHandler OnRightClick;
        public event OnClickHandler OnMiddleClick;

        private ImageSource _itemIcon;

        public ImageSource ItemIconPath
        {
            get { return _itemIcon; }
            set { _itemIcon = value; OnPropertyChanged(); }
        }

        private ImageSource _aowIcon;

        public ImageSource AshOfWarIcon
        {
            get { return _aowIcon; }
            set { _aowIcon = value; OnPropertyChanged(); }
        }
        private ImageSource _infIcon;

        public ImageSource InfusionIconPath
        {
            get { return _infIcon; }
            set { _infIcon = value; OnPropertyChanged(); }
        }

        private Visibility _visibility;

        public Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; OnPropertyChanged(); }
        }

        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; OnPropertyChanged(); }
        }

        private string _upgradeLevel;

        public string UpgradeLevel
        {
            get { return _upgradeLevel; }
            set { _upgradeLevel = value; OnPropertyChanged(); }
        }


        public BuildItem Item { get; set; }

        public ICommand ClickCommand { get; set; }
        public ICommand RightClickCommand { get; set; }
        public ICommand MiddleClickCommand { get; set; }

        public InventoryItemModel(BuildItem item, ImageSource itemIconPath, ImageSource ashOfWarIcon, ImageSource infusionIconPath, string itemName, string upgradeLevel = "")
        {
            ItemIconPath = itemIconPath;
            AshOfWarIcon = ashOfWarIcon;
            InfusionIconPath = infusionIconPath;
            ItemName = itemName;
            UpgradeLevel = upgradeLevel;
            Item = item;

            ClickCommand = new RelayCommand((o) => OnLeftClick?.Invoke(Item));
            RightClickCommand = new RelayCommand((o) => OnRightClick?.Invoke(Item));
            MiddleClickCommand = new RelayCommand((o) => OnMiddleClick?.Invoke(Item));
        }

        /*public void SetupFromItem(Item item)
        {
            if (item == null)
            {
                Visibility = Visibility.Hidden;
                return;
            }
            Visibility = Visibility.Visible;
            ItemIconPath = Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID));
            ItemName = item.Name;
        }*/

    }
}
