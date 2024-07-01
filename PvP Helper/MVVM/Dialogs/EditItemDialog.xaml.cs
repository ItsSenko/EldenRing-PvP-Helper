using Erd_Tools.Models;
using PvPHelper.Core;
using PvPHelper.MVVM.Models;
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

            this.Loaded += EditItemDialog_Loaded;
        }



        private object _ashSelectedItem;

        public object AshSelectedItem
        {
            get { return _ashSelectedItem; }
            set 
            {
                _ashSelectedItem = value;
                OnPropertyChanged();
                OnAshChanged(value);
            }
        }



        private void EditItemDialog_Loaded(object sender, RoutedEventArgs e)
        {
            UpgradeBox.CurrValue = Prefab.UpgradeLevel;
            UpgradeBox.InputText = Prefab.UpgradeLevel.ToString();

            List<GemOption> gemOptions = new();
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
                                gemOptions.Add(new(gem.Name.Contains("Ash of War: ") ? gem.Name.Substring(12) : gem.Name, gem));
                            }
                        }
                        AshOfWarBox.ItemsSource = gemOptions;
                        UpgradeBox.Max = 25;
                    }
                    else
                        UpgradeBox.Max = 10;
                }
            }
            AshOfWarBox.SelectedIndex = gemOptions.IndexOf(gemOptions.FirstOrDefault(gem => gem.gem.ID == Prefab.SwordArtID));

            GemOption option = AshOfWarBox.SelectedItem as GemOption;
            if (option != null)
            {
                List<InfusionOption> infusionOptions = new();
                foreach (Infusion infusion in option.gem.Infusions)
                    infusionOptions.Add(new(infusion.ToString(), infusion));
                InfusionBox.ItemsSource = infusionOptions;
                InfusionBox.SelectedIndex = infusionOptions.IndexOf(infusionOptions.FirstOrDefault(inf => (int)inf.infusion == Prefab.Infusion));
            }
            //AshOfWarBox.OnSelectedItemChanged += AshOfWarBox_OnSelectedItemChanged;
        }

        private WeaponPrefab _prefab;

        public WeaponPrefab Prefab
        {
            get { return _prefab; }
            set { _prefab = value; }
        }

        public event Action OnCancel = new(() => { });
        public event Action<WeaponPrefab> OnSubmit = new((prefab) => { });

        public void OnAshChanged(object obj)
        {
            if (obj == null)
                return;

            //InfusionBox.OriginItems = null;
            GemOption option = obj as GemOption;
            List<InfusionOption> infusionOptions = new();
            foreach (Infusion infusion in option.gem.Infusions)
                infusionOptions.Add(new(infusion.ToString(), infusion));
            InfusionBox.ItemsSource = infusionOptions;
        }

        private void AshOfWarBox_OnSelectedItemChanged(object obj)
        {
            if (obj == null)
                return;

            //InfusionBox.OriginItems = null;
            GemOption option = obj as GemOption;
            List<InfusionOption> infusionOptions = new();
            foreach (Infusion infusion in option.gem.Infusions)
                infusionOptions.Add(new(infusion.ToString(), infusion));
            InfusionBox.ItemsSource = infusionOptions;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            InfusionOption infOption = InfusionBox.SelectedItem as InfusionOption;
            GemOption gemOption = AshOfWarBox.SelectedItem as GemOption;
            WeaponPrefab prefab = new(Prefab.Name, Prefab.ID, infOption == null ? Infusion.Standard : infOption.infusion, gemOption == null ? -1 : gemOption.gem.ID, UpgradeBox.CurrValue);
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
