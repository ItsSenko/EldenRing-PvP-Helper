using Erd_Tools;
using Erd_Tools.Models;
using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for NetPlayerControl.xaml
    /// </summary>
    public partial class NetPlayerControl : UserControl
    {
        public NetPlayer Player;
        private NetPlayer LocalPlayer;
        private ErdHook hook;
        private string ItemPicFolder = "PvPHelper.Resources.Images.Items";
        private ItemCategory TalismanCat;
        public NetPlayerControl()
        {
            InitializeComponent();

            TalismanCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");
            this.MouseDoubleClick += OpenMoreInfo;
        }

        public void Setup(NetPlayer player, NetPlayer localPlayer, ErdHook hook)
        {
            Player = player;
            LocalPlayer = localPlayer;
            this.hook = hook;
            Name.Text = Player.Name;
            Level.Text = Player.Level.ToString();

            HealthBar.Maximum = Player.HPMax;
            HealthBar.Value = Player.Health;
            HealthBlock.Text = Player.Health.ToString();

            var tal1Item = Helpers.GetItemFromID(TalismanCat, Player.Accessory1ID);
            var tal2Item = Helpers.GetItemFromID(TalismanCat, Player.Accessory2ID);
            var tal3Item = Helpers.GetItemFromID(TalismanCat, Player.Accessory3ID);
            var tal4Item = Helpers.GetItemFromID(TalismanCat, Player.Accessory4ID);

            Tal1.Source = tal1Item != null ? Helpers.GetImageSource(tal1Item.Name) : null;
            Tal2.Source = tal2Item != null ? Helpers.GetImageSource(tal2Item.Name) : null;
            Tal3.Source = tal3Item != null ? Helpers.GetImageSource(tal3Item.Name) : null;
            Tal4.Source = tal4Item != null ? Helpers.GetImageSource(tal4Item.Name) : null;

            Tal1.ToolTip = tal1Item != null ? tal1Item.Name : "Empty";
            Tal2.ToolTip = tal2Item != null ? tal2Item.Name : "Empty";
            Tal3.ToolTip = tal3Item != null ? tal3Item.Name : "Empty";
            Tal4.ToolTip = tal4Item != null ? tal4Item.Name : "Empty";

            
        }
        public void UpdateUI()
        {
            HealthBar.Maximum = Player.HPMax;
            HealthBar.Value = Player.Health;
            HealthBlock.Text = Player.Health.ToString();

            var tal1Item = Helpers.GetItemFromID(TalismanCat, Player.Accessory1ID);
            var tal2Item = Helpers.GetItemFromID(TalismanCat, Player.Accessory2ID);
            var tal3Item = Helpers.GetItemFromID(TalismanCat, Player.Accessory3ID);
            var tal4Item = Helpers.GetItemFromID(TalismanCat, Player.Accessory4ID);

            Tal1.Source = tal1Item != null ? Helpers.GetImageSource(tal1Item.Name) : null;
            Tal2.Source = tal2Item != null ? Helpers.GetImageSource(tal2Item.Name) : null;
            Tal3.Source = tal3Item != null ? Helpers.GetImageSource(tal3Item.Name) : null;
            Tal4.Source = tal4Item != null ? Helpers.GetImageSource(tal4Item.Name) : null;

            Tal1.ToolTip = tal1Item != null ? tal1Item.Name : "Empty";
            Tal2.ToolTip = tal2Item != null ? tal2Item.Name : "Empty";
            Tal3.ToolTip = tal3Item != null ? tal3Item.Name : "Empty";
            Tal4.ToolTip = tal4Item != null ? tal4Item.Name : "Empty";
        }

        
        private void OpenMoreInfo(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                NetPlayerDialog dialog = new(Player, LocalPlayer, hook);
                dialog.ShowDialog();
            }
        }
    }
}
