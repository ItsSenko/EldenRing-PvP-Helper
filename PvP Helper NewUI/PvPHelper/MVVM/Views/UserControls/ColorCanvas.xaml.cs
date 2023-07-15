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
    /// Interaction logic for ColorCanvas.xaml
    /// </summary>
    public partial class ColorCanvas : UserControl
    {
        public static readonly DependencyProperty selectedColor =
                   DependencyProperty.Register(
                         "SelectedColor",
                          typeof(Color),
                          typeof(ColorCanvas));
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(selectedColor);
            }
            set
            {
                SetValue(selectedColor, value);
            }
        }

        public static readonly RoutedEvent SelectedColorChangedEvent =
        EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<Color?>), typeof(ColorCanvas));

        public event RoutedPropertyChangedEventHandler<Color?> SelectedColorChanged
        {
            add { AddHandler(SelectedColorChangedEvent, value); }
            remove { RemoveHandler(SelectedColorChangedEvent, value); }
        }
        public ColorCanvas()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void ColorCanvas_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            RoutedPropertyChangedEventArgs<Color?> newE = new RoutedPropertyChangedEventArgs<Color?>(null, SelectedColor, SelectedColorChangedEvent);
            RaiseEvent(newE);
        }
    }
}
