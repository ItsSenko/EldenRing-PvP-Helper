using PvPHelper.MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SimpleDropDown.xaml
    /// </summary>
    public partial class SimpleDropDown : UserControl, INotifyPropertyChanged
    {
        public SimpleDropDown()
        {
            DataContext = this;
            InitializeComponent();
        }
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region Data Bindings
        public event Action<BaseOption> OnSelectedItemChanged;
        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged.Invoke((BaseOption)value);
            }
        }
        private IEnumerable<object> _itemsSource;

        public IEnumerable<object> ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                _itemsSource = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
