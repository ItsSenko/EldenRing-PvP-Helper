using PvPHelper.MVVM.Models.Regions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for LabeledCircleToggle.xaml
    /// </summary>
    public partial class RegionToggle : UserControl
    {
        public PlayRegion PlayRegion;
        public RegionToggle()
        {
            InitializeComponent();

            Toggle.Checked += Toggle_Checked;
            Toggle.Unchecked += Toggle_Unchecked;
            this.Loaded += RegionToggle_Loaded;
        }

        private void RegionToggle_Loaded(object sender, RoutedEventArgs e)
        {
            Toggle.IsChecked = PlayRegion.IsActive;
        }

        private void Toggle_Checked(object sender, RoutedEventArgs e)
        {
            PlayRegion.IsActive = true;
        }
        private void Toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            PlayRegion.IsActive = false;
        }
    }
}
