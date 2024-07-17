using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Items;
using PvPHelper.Core;
using PvPHelper.Core.Extensions;
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
using static Erd_Tools.Models.Weapon;

namespace PvPHelper.MVVM.Models
{
    public class ItemGibModel : ViewModelBase
    {
        public delegate void OnClickHandler(Item item);
        public event OnClickHandler OnLeftClick;
        public event OnClickHandler OnRightClick;


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

        public string UpgradeLevel { get; set; }

        public Item Item { get; set; }
        public Infusion Infusion { get; set; }

        public ICommand ClickCommand { get; set; }
        public ICommand RightClickCommand { get; set; }

        public ItemGibModel(ImageSource itemIconPath, ImageSource ashOfWarIcon, ImageSource infusionIconPath, string itemName, string upgradeLevel = "")
        {
            ItemIconPath = itemIconPath;
            AshOfWarIcon = ashOfWarIcon;
            InfusionIconPath = infusionIconPath;
            ItemName = itemName;
            UpgradeLevel = upgradeLevel;

            ClickCommand = new RelayCommand((o) => OnLeftClick?.Invoke(Item));
            RightClickCommand = new RelayCommand((o) => OnRightClick?.Invoke(Item));
        }

        public ItemGibModel(Infusion infusion, ImageSource Icon, string Name)
        {
            Infusion = infusion;
            ItemIconPath = Icon;
            ItemName = Name;
        }

        public void SetupFromItem(Item item)
        {
            if (item == null)
            {
                Visibility = Visibility.Hidden;
                return;
            }
            Visibility = Visibility.Visible;
            ItemIconPath = Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID));
            ItemName = item.Name;
            Item = item;
        }

    }
}
