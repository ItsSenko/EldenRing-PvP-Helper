using Erd_Tools;
using Erd_Tools.Models;
using PvPHelper.Console;
using PvPHelper.Core;
using PvPHelper.MVVM.Commands.Dashboard.Toggles;
using PvPHelper.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Erd_Tools.Models.Param;
using CommandManager = PvPHelper.Console.CommandManager;

namespace PvPHelper.MVVM.Dialogs
{
    /// <summary>
    /// Interaction logic for NetPlayerDialog.xaml
    /// </summary>
    public partial class NetPlayerDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<PhantomIDOption> _phantomIdItemsSource;

        public ObservableCollection<PhantomIDOption> PhantomIDItemsSource
        {
            get { return _phantomIdItemsSource; }
            set { _phantomIdItemsSource = value; OnPropertyChanged(); }
        }

        private object _selectedPhantomID;

        public object SelectedPhantomID
        {
            get { return _selectedPhantomID; }
            set { _selectedPhantomID = value; OnPropertyChanged(); OnSelectedPhantomIDChanged(); }
        }

        private ErdHook hook;
        private NetPlayer Player;
        private NetPlayer LocalPlayer;
        private ItemCategory Talismans;
        private ItemCategory Armors;
        private DispatcherTimer UpdateTimer;
        private Param PhantomParam => hook.Params.FirstOrDefault(x => x.Name == "PhantomParam");
        private ItemCategory[] WeaponsCategories;
        public NetPlayerDialog(NetPlayer player, NetPlayer localPlayer, ErdHook hook)
        {
            InitializeComponent();
            DataContext = this;
            Player = player;
            LocalPlayer = localPlayer;
            this.hook = hook;

            Talismans = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");
            Armors = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");

            var shields = ItemCategory.All.FirstOrDefault(x => x.Name == "Shields");
            var spellTools = ItemCategory.All.FirstOrDefault(x => x.Name == "Spell Tools");
            var rangedWpns = ItemCategory.All.FirstOrDefault(x => x.Name == "Ranged Weapons");
            var meleeWpns = ItemCategory.All.FirstOrDefault(x => x.Name == "Melee Weapons");
            WeaponsCategories = new ItemCategory[] { shields, spellTools, rangedWpns, meleeWpns};

            PhantomIDItemsSource = new();
            PhantomIDItemsSource.Add(new(Helpers.GetNewPhantomName(-1), -1));
            foreach(var row in PhantomParam.Rows)
            {
                PhantomIDItemsSource.Add(new(Helpers.GetNewPhantomName(row.ID), row.ID));
            }

            UpdateTimer = new();
            UpdateTimer.Interval = TimeSpan.FromSeconds(1);
            UpdateTimer.Tick += UpdateTimer_Tick;
            Setup();
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            Name.Text = Player.Name;
            Level.Text = $"Lvl {Player.Level}";

            Vigor.Text = $"Vigor: {Player.Vigor}";
            Mind.Text = $"Mind: {Player.Mind}";
            End.Text = $"Endurance: {Player.Endurance}";
            Str.Text = $"Strength: {Player.Strength}";
            Dex.Text = $"Dexterity: {Player.Dexterity}";
            Int.Text = $"Intelligence: {Player.Intelligence}";
            Faith.Text = $"Faith: {Player.Faith}";
            Arc.Text = $"Arcane: {Player.Arcane}";

            Item helmet = Helpers.GetItemFromID(Armors, Player.HelmetID);
            Item armor = Helpers.GetItemFromID(Armors, Player.ArmorID);
            Item gauntlet = Helpers.GetItemFromID(Armors, Player.GauntletID);
            Item leggings = Helpers.GetItemFromID(Armors, Player.LeggingsID);

            //Item rWpn1 = Helpers.GetItemFromID(WeaponsCategories, Helpers.GetProperID(Player.RWeapon1));
            //Item rWpn2 = Helpers.GetItemFromID(WeaponsCategories, Helpers.GetProperID(Player.RWeapon2));
            //Item rWpn3 = Helpers.GetItemFromID(WeaponsCategories, Helpers.GetProperID(Player.RWeapon3));

            //Item lWpn1 = Helpers.GetItemFromID(WeaponsCategories, Helpers.GetProperID(Player.LWeapon1));
            //Item lWpn2 = Helpers.GetItemFromID(WeaponsCategories, Helpers.GetProperID(Player.LWeapon2));
            //Item lWpn3 = Helpers.GetItemFromID(WeaponsCategories, Helpers.GetProperID(Player.LWeapon3));

            Helmet.Source = helmet != null ? Helpers.GetImageSource(Helpers.GetFullIconID(helmet.IconID)) : null;
            Armor.Source = armor != null ? Helpers.GetImageSource(Helpers.GetFullIconID(armor.IconID)) : null;
            Gauntlets.Source = gauntlet != null ? Helpers.GetImageSource(Helpers.GetFullIconID(gauntlet.IconID)) : null;
            Leggings.Source = leggings != null ? Helpers.GetImageSource(Helpers.GetFullIconID(leggings.IconID)) : null;

            //RWpn1.Source = rWpn1 != null ? Helpers.GetImageSource(Helpers.GetFullIconID(rWpn1.IconID)) : null;
            //RWpn2.Source = rWpn2 != null ? Helpers.GetImageSource(Helpers.GetFullIconID(rWpn2.IconID)) : null;
            //RWpn3.Source = rWpn3 != null ? Helpers.GetImageSource(Helpers.GetFullIconID(rWpn3.IconID)) : null;

            //LWpn1.Source = lWpn1 != null ? Helpers.GetImageSource(Helpers.GetFullIconID(lWpn1.IconID)) : null;
            //LWpn2.Source = lWpn2 != null ? Helpers.GetImageSource(Helpers.GetFullIconID(lWpn2.IconID)) : null;
            //LWpn3.Source = lWpn3 != null ? Helpers.GetImageSource(Helpers.GetFullIconID(lWpn3.IconID)) : null;

            Helmet.ToolTip = helmet != null ? helmet.Name : "Empty";
            Armor.ToolTip = armor != null ? armor.Name : "Empty";
            Gauntlets.ToolTip = gauntlet != null ? gauntlet.Name : "Empty";
            Leggings.ToolTip = leggings != null ? leggings.Name : "Empty";

            //RWpn1.ToolTip = rWpn1 != null ? rWpn1.Name : "Empty";
            //RWpn2.ToolTip = rWpn2 != null ? rWpn2.Name : "Empty";
            //RWpn3.ToolTip = rWpn3 != null ? rWpn3.Name : "Empty";

            //LWpn1.ToolTip = lWpn1 != null ? lWpn1.Name : "Empty";
            //LWpn2.ToolTip = lWpn2 != null ? lWpn2.Name : "Empty";
            //LWpn3.ToolTip = lWpn3 != null ? lWpn3.Name : "Empty";

            XBlock.Text = $"X: {Math.Round(Player.LocalX, 1)}";
            YBlock.Text = $"Y: {Math.Round(Player.LocalY, 1)}";
            ZBlock.Text = $"Z: {Math.Round(Player.LocalZ, 1)}";
        }

        public void Setup()
        {
            UpdateTimer.Start();

            SelectedPhantomID = PhantomIDItemsSource.FirstOrDefault(x => x.ID == Player.PhantomID);

            XBtn.Click += (s, e) => { this.Close(); };
            ChangeClrBtn.Click += (s, e) => { ChangeColor(); };
            TpToBtn.Click += (s, e) => { LocalPlayer.TeleportToPlayer(Player); };
            KickBtn.Click += (s, e) => { Player.Kick(); };

            if (CustomPointers.SessionMan.ReadInt32(0xC) != 3)
                KickBtn.IsEnabled = true;
        }

        public void OnSelectedPhantomIDChanged()
        {
            PhantomIDOption option = SelectedPhantomID as PhantomIDOption;

            if (option == null)
                return;

            Player.PhantomID = option.ID;
        }

        public int edgeColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorR").FieldOffset;
        public int edgeColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorG").FieldOffset;
        public int edgeColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorB").FieldOffset;
        public int diffMulColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorR").FieldOffset;
        public int diffMulColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorG").FieldOffset;
        public int diffMulColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorB").FieldOffset;
        public int frontColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorR").FieldOffset;
        public int frontColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorG").FieldOffset;
        public int frontColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorB").FieldOffset;

        public void ChangeColor()
        {
            if (SelectedPhantomID == null)
                return;
            ColorPickerDialog dialog = new();
            PhantomIDOption phantom = SelectedPhantomID as PhantomIDOption;
            dialog.OnSubmit += (color) =>
            {
                if (phantom == null || phantom.ID == -1)
                    return;

                Row row = PhantomParam.Rows.FirstOrDefault(x => x.ID == phantom.ID);
                var dataOffset = row.DataOffset;

                row.Param.Pointer.WriteByte((int)dataOffset + edgeColorR, color.R);
                row.Param.Pointer.WriteByte((int)dataOffset + edgeColorG, color.G);
                row.Param.Pointer.WriteByte((int)dataOffset + edgeColorB, color.B);

                row.Param.Pointer.WriteByte((int)dataOffset + diffMulColorR, 255);
                row.Param.Pointer.WriteByte((int)dataOffset + diffMulColorG, 255);
                row.Param.Pointer.WriteByte((int)dataOffset + diffMulColorB, 255);

                Color invertedColor = invertColor(color);
                row.Param.Pointer.WriteByte((int)dataOffset + frontColorR, (byte)Math.Round(invertedColor.R * 0.3, 0));
                row.Param.Pointer.WriteByte((int)dataOffset + frontColorG, (byte)Math.Round(invertedColor.G * 0.3, 0));
                row.Param.Pointer.WriteByte((int)dataOffset + frontColorB, (byte)Math.Round(invertedColor.B * 0.3, 0));

                CommandManager.Log("Color Updated");
                CommandManager.Log($"R: {color.R} G: {color.G} B: {color.B}");
            };
            dialog.ShowDialog();
        }
        private Color invertColor(Color colorToInvert)
        {
            return Color.FromArgb(colorToInvert.ToArgb() ^ 0xffffff);
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
