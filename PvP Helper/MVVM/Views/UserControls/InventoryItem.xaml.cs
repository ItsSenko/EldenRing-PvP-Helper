using Erd_Tools.Models;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.Models.Builds;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for InventoryItem.xaml
    /// </summary>
    ///
    public class NullImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
    public partial class InventoryItem : UserControl
    {
        public InventoryItem()
        {
            InitializeComponent();
            //PreviewMouseLeftButtonDown += InventoryItem_PreviewMouseLeftButtonDown;
            DataContext = this;
        }
        public string Name { get; set; }
        public BuildItem Item { get; set; }
        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(ItemNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemNameProperty =
            DependencyProperty.Register("ItemName", typeof(string), typeof(InventoryItem));

        
        public ImageSource ItemIconPath
        {
            get { return (ImageSource)GetValue(ItemIconPathProperty); }
            set { SetValue(ItemIconPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemIconPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemIconPathProperty =
            DependencyProperty.Register("ItemIconPath", typeof(ImageSource), typeof(InventoryItem));



        public ImageSource AshOfWarIcon
        {
            get { return (ImageSource)GetValue(AshOfWarIconProperty); }
            set { 
                SetValue(AshOfWarIconProperty, value);
                if (AshOfWarIcon == null)
                    GemUI.Visibility = Visibility.Hidden;
                else
                    GemUI.Visibility = Visibility.Visible;
            }
        }

        // Using a DependencyProperty as the backing store for AshOfWarIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AshOfWarIconProperty =
            DependencyProperty.Register("AshOfWarIcon", typeof(ImageSource), typeof(InventoryItem));


        public string UpgradeLevel
        {
            get { return (string)GetValue(UpgradeLevelProperty); }
            set { SetValue(UpgradeLevelProperty, value);
                if (string.IsNullOrWhiteSpace(UpgradeLevel))
                    UpgradeUI.Visibility = Visibility.Hidden;
                else
                    UpgradeUI.Visibility = Visibility.Visible;
            }
        }

        // Using a DependencyProperty as the backing store for UpgradeLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpgradeLevelProperty =
            DependencyProperty.Register("UpgradeLevel", typeof(string), typeof(InventoryItem));

        public ImageSource InfusionIconPath
        {
            get { return (ImageSource)GetValue(InfusionIconPathProperty); }
            set { SetValue(InfusionIconPathProperty, value);
                if (InfusionIconPath == null)
                    InfusionUI.Visibility = Visibility.Hidden;
                else
                    InfusionUI.Visibility = Visibility.Visible;
            }
        }

        // Using a DependencyProperty as the backing store for InfusionIconPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InfusionIconPathProperty =
            DependencyProperty.Register("InfusionIconPath", typeof(ImageSource), typeof(InventoryItem));

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            borderActive.Opacity = 1;
            borderInActive.Opacity = 0;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            borderActive.Opacity = 0;
            borderInActive.Opacity = 1;
        }
    }
}
