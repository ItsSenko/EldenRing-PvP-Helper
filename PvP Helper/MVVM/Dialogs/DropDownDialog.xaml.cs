using PvPHelper.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PvPHelper.MVVM.Dialogs
{
    /// <summary>
    /// Interaction logic for InformationDialog.xaml
    /// </summary>
    public partial class DropDownDialog : Window
    {
        public DropDownDialog(string title, List<DropDownObject<object>> source)
        {
            InitializeComponent();

            Text.Text = title;
            DropDown.ItemsSource = source;
        }
        public event Action OnCancel = new(() => { });
        public event Action<object> OnOk = new((obj) => { }); 
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnOk.Invoke(DropDown.SelectedItem);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnCancel.Invoke();
        }
    }
}
