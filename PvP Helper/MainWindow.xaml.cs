using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
