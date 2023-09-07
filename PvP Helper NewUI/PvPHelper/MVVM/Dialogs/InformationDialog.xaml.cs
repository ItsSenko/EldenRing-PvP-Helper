using System;
using System.Windows;

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
