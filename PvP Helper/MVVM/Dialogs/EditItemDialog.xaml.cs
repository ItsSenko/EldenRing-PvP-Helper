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
        private SearchAlgorithm<NamedObject<Infusion>> infusionSearch;

        private void EditItemDialog_Loaded()
        {
            gemSearch = new(new List<Gem>(), new AlphabeticalSort<Gem>());
            gemSearch.OnItemsChanged += (items) => { AshOfWarBox.ItemsSource = items; };
            infusionSearch = new(new List<NamedObject<Infusion>>(), new AlphabeticalSort<NamedObject<Infusion>>());
            infusionSearch.OnItemsChanged += (items) => { InfusionBox.ItemsSource = items; };

            UpgradeBox.CurrValue = Prefab.UpgradeLevel;
            UpgradeBox.InputText = Prefab.UpgradeLevel.ToString();

            List<Gem> gemOptions = new();
            ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Items.FirstOrDefault(x => x.ID == Prefab.ID && x is Weapon) != null);
            if (category != null)
            {
                Item item = category.Items.FirstOrDefault(x => x.ID == Prefab.ID);
                if (item is Weapon wpn)
                {
                    UpgradeBox.Max = wpn.MaxUpgrade;
                    if (wpn.GemAttachType == GemMountType.Any)
                    {
                        foreach (Gem gem in Gem.All)
                        {
                            if (gem.WeaponTypes.Contains(wpn.Type))
                            {
                                gemOptions.Add(gem);
                            }
                        }
                        gemSearch.Items = gemOptions;
                    }
                        

                    Gem option = AshOfWarBox.SelectedItem as Gem;
                    if (option != null && wpn.GemAttachType == GemMountType.Any)
                    {
                        List<NamedObject<Infusion>> infusionOptions = new();
                        foreach (Infusion infusion in option.Infusions)
                            infusionOptions.Add(new(infusion, infusion.ToString()));
                        infusionSearch.Items = infusionOptions;
                        InfusionBox.SelectedIndex = infusionOptions.IndexOf(infusionOptions.FirstOrDefault(inf => (int)inf.Value == Prefab.Infusion));
                    }
                    AshOfWarBox.SelectedIndex = gemOptions.IndexOf(gemOptions.FirstOrDefault(gem => gem.ID == Prefab.SwordArtID));
                }
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
            Gem option = obj as Gem;
            List<NamedObject<Infusion>> infusionOptions = new();
            foreach (Infusion infusion in option.Infusions)
                infusionOptions.Add(new(infusion, infusion.ToString()));
            infusionSearch.Items = infusionOptions;
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            NamedObject<Infusion> infOption = (NamedObject<Infusion>)InfusionBox.SelectedItem;
            Gem gemOption = AshOfWarBox.SelectedItem as Gem;
            WeaponItem prefab = new(Prefab.Name, Prefab.ID, Prefab.IconID, Prefab.Category,
                infOption == null ? (int)Infusion.Standard : (int)infOption.Value,
                infOption == null ? "" : infOption.ToString(),
                gemOption == null ? -1 : gemOption.ID,
                gemOption == null ? (short)0 : gemOption.IconID,
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
