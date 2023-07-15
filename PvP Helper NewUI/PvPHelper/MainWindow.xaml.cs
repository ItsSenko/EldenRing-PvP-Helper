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

namespace PvPHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void HighLightColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Application.Current.Resources["Highlight"] = e.NewValue;
            ((LinearGradientBrush)Application.Current.Resources["DashHighlightGradient"]).GradientStops[0].Color = (Color)(e.NewValue);
        }
    }
}
