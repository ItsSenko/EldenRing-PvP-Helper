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
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PvPHelper.MVVM.Dialogs
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog : Window
    {
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        public event Action OnCancel = new(() => { });
        public event Action<Color> OnSubmit = new((c) => { });
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*if (e.ChangedButton == MouseButton.Left)
                this.DragMove();*/
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnCancel.Invoke();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnCancel.Invoke();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Color newColor = Color.FromArgb(ColorPicker.SelectedColor.A, ColorPicker.SelectedColor.R, ColorPicker.SelectedColor.G, ColorPicker.SelectedColor.B);
            OnSubmit.Invoke(newColor);
        }
    }
}
