using System;
using System.Windows;
using System.Windows.Input;
using System.Drawing;

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
