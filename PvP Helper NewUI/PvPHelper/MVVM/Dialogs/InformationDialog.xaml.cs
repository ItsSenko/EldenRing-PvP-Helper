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
    /// Interaction logic for InformationDialog.xaml
    /// </summary>
    public partial class InformationDialog : Window
    {
        public InformationDialog(string title)
        {
            InitializeComponent();

            MainBlock.Text = title;
        }
        public event Action OnCancel = new(() => { });
        public event Action OnOk = new(() => { }); 
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnOk.Invoke();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnCancel.Invoke();
        }
    }
}
