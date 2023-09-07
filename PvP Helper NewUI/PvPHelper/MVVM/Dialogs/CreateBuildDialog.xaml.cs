using PvPHelper.Core;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PvPHelper.MVVM.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateBuildDialog.xaml
    /// </summary>
    public partial class CreateBuildDialog : Window
    {
        public event Action OnCancel = new(() => { });
        public event Action<string> OnSave = new((s) => { });
        public CreateBuildDialog()
        {
            InitializeComponent();
            TextBox.Text = "Build Name...";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!Helpers.IsValidFileName(TextBox.Text))
            {
                MessageBox.Show("Invalid File Name", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Close();
            OnSave.Invoke(TextBox.Text);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnCancel.Invoke();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBox.Text != "Build Name...")
                return;

            TextBox.Text = string.Empty;
        }
    }
}
