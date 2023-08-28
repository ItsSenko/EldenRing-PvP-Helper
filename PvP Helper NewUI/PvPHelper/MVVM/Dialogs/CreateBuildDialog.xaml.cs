using PvPHelper.Core;
using PvPHelper.MVVM.Models;
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
using System.Windows.Shapes;

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
