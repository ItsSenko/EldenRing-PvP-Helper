using Erd_Tools;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PvPHelper.MVVM.Models;
using Erd_Tools.Models;
using static Erd_Tools.Models.Weapon;
using PvPHelper.MVVM.Views.UserControls;
using System.Collections.ObjectModel;
using PvPHelper.MVVM.Commands.PrefabCreator;
using System.Windows.Media;
using PvPHelper.MVVM.Dialogs;
using PvPHelper.MVVM.Models.Search.SortOrders;
using PvPHelper.MVVM.Models.Search;
using PvPHelper.MVVM.Models.Builds;
using System.ComponentModel;
using System.Windows;
using System.Threading;
using Erd_Tools.Models.Items;

namespace PvPHelper.MVVM.ViewModels
{
    public class ItemGiveViewModel : ViewModelBase
    {
        private ObservableCollection<InventoryItemModel> _inventoryItems;

        public ObservableCollection<InventoryItemModel> InventoryItems
        {
            get { return _inventoryItems; }
            set { _inventoryItems = value; OnPropertyChanged(); }
        }
        public ICommand RefreshBuilds { get; set; }
        private ErdHook hook;
        public ItemGiveViewModel(ErdHook hook)
        {
            this.hook = hook;
            InventoryItems = new();
            RefreshBuilds = new RelayCommand(o =>
            {
                
                ObservableCollection<InventoryItemModel> list = new();
                foreach (var cat in ItemCategory.All)
                {
                    foreach (var item in cat.Items)
                    {
                        list.Add(new(Helpers.GetImageSource(Helpers.GetFullIconID(item.IconID)), null, null, item.Name));
                    }
                }
                InventoryItems = list;
            });
            
        }
    }
}
