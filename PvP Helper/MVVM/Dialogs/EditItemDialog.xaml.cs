using Erd_Tools.Models;
using PvPHelper.Core;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.Models.Builds;
using PvPHelper.MVVM.Models.Search;
using PvPHelper.MVVM.Models.Search.SortOrders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Erd_Tools.Models.Weapon;

namespace PvPHelper.MVVM.Dialogs
{
    /// <summary>
    /// Interaction logic for EditItemDialog.xaml
    /// </summary>
    public partial class EditItemDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public EditItemDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private object _infusionSelectedItem;

        public object InfusionSelectedItem
        {
            get { return _infusionSelectedItem; }
            set 
            { 
                _infusionSelectedItem = value;
                if (value == null && !string.IsNullOrEmpty(InfSearchText) && infusionSearch != null && infusionSearch.ShownItems != null)
                {
                    _infusionSelectedItem = infusionSearch.ShownItems.FirstOrDefault(x => x.ToString() == InfSearchText);
                }
                OnPropertyChanged();
            }
        }


        private object _ashSelectedItem;

        public object AshSelectedItem
        {
            get { return _ashSelectedItem; }
            set 
            {
                _ashSelectedItem = value;
                if (value == null && !string.IsNullOrEmpty(AshSearchText) && gemSearch != null && gemSearch.ShownItems != null)
                {
                    _ashSelectedItem = gemSearch.ShownItems.FirstOrDefault(x => x.ToString() == AshSearchText);
                }
                OnPropertyChanged();
                OnAshChanged(value);
            }
        }

        private string _ashSearch;

        public string AshSearchText
        {
            get { return _ashSearch; }
            set { _ashSearch = value;
                if (gemSearch != null)
                    gemSearch.SearchString = _ashSearch;
            }
        }

        private string _infSearch;

        public string InfSearchText
        {
            get { return _infSearch; }
            set { _infSearch = value;
                if (infusionSearch != null)
                    infusionSearch.SearchString = _infSearch;
            }
        }

        private SearchAlgorithm<Gem> gemSearch;
        private SearchAlgorithm<Infusion> infusionSearch;

        private void EditItemDialog_Loaded()
        {
            gemSearch = new(new(), new AlphabeticalSort<Gem>());
            gemSearch.OnItemsChanged += (items) => { AshOfWarBox.ItemsSource = items; };
            infusionSearch = new(new(), new AlphabeticalSort<Infusion>());
            infusionSearch.OnItemsChanged += (items) => { InfusionBox.ItemsSource = items; };

            UpgradeBox.CurrValue = Prefab.UpgradeLevel;
            UpgradeBox.InputText = Prefab.UpgradeLevel.ToString();

            List<SearchItem<Gem>> gemOptions = new();
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Items.FirstOrDefault(x => x.ID == Prefab.ID && x is Weapon) != null);
            if (category != null)
            {
                Item item = category.Items.FirstOrDefault(x => x.ID == Prefab.ID);
                if (item is Weapon wpn)
                {
                    if (!wpn.Unique)
                    {
                        foreach (Gem gem in Gem.All)
                        {
                            if (gem.WeaponTypes.Contains(wpn.Type))
                            {
                                gemOptions.Add(new(gem, gem.Name.Contains("Ash of War: ") ? gem.Name.Substring(12) : gem.Name));
                            }
                        }
                        gemSearch.Items = gemOptions;
                        UpgradeBox.Max = 25;
                    }
                    else
                        UpgradeBox.Max = 10;
                }
            }
            AshOfWarBox.SelectedIndex = gemOptions.IndexOf(gemOptions.FirstOrDefault(gem => gem.Item.ID == Prefab.SwordArtID));

            SearchItem<Gem> option = AshOfWarBox.SelectedItem as SearchItem<Gem>;
            if (option != null)
            {
                List<SearchItem<Infusion>> infusionOptions = new();
                foreach (Infusion infusion in option.Item.Infusions)
                    infusionOptions.Add(new(infusion, infusion.ToString()));
                infusionSearch.Items = infusionOptions;
                InfusionBox.SelectedIndex = infusionOptions.IndexOf(infusionOptions.FirstOrDefault(inf => (int)inf.Item == Prefab.Infusion));
            }
            //AshOfWarBox.OnSelectedItemChanged += AshOfWarBox_OnSelectedItemChanged;
        }

        private WeaponItem _prefab;

        public WeaponItem Prefab
        {
            get { return _prefab; }
            set { 
                if (_prefab == null && value != null)
                {
                    _prefab = value;
                    EditItemDialog_Loaded();
                }
                else
                    _prefab = value;  
            }
        }

        public event Action OnCancel = new(() => { });
        public event Action<WeaponItem> OnSubmit = new((prefab) => { });

        public void OnAshChanged(object obj)
        {
            if (obj == null)
                return;
            SearchItem<Gem> option = obj as SearchItem<Gem>;
            List<SearchItem<Infusion>> infusionOptions = new();
            foreach (Infusion infusion in option.Item.Infusions)
                infusionOptions.Add(new(infusion, infusion.ToString()));
            infusionSearch.Items = infusionOptions;
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SearchItem<Infusion> infOption = InfusionBox.SelectedItem as SearchItem<Infusion>;
            SearchItem<Gem> gemOption = AshOfWarBox.SelectedItem as SearchItem<Gem>;
            WeaponItem prefab = new(Prefab.Name, Prefab.ID, Prefab.IconID, Prefab.Category,
                infOption == null ? (int)Infusion.Standard : (int)infOption.Item,
                infOption == null ? "" : infOption.Item.ToString(),
                gemOption == null ? -1 : gemOption.Item.ID,
                gemOption == null ? (short)0 : gemOption.Item.IconID,
                UpgradeBox.CurrValue);
            OnSubmit.Invoke(prefab);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OnCancel.Invoke();
            this.Close();
        }
    }
}
