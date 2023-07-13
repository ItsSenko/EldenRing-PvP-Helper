using PvPHelper.MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SearchableComboBox.xaml
    /// </summary>
    public partial class SearchableComboBox : UserControl, INotifyPropertyChanged
    {
        public SearchableComboBox()
        {
            DataContext = this;
            InitializeComponent();
            Placeholder = "Search...";


        }
        #region DataBindings
        public event Action<BaseOption> OnSelectedChanged;
        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnSelectedChanged?.Invoke((BaseOption)value);
            }
        }
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
        private string _placeholder;

        public string Placeholder
        {
            get { return _placeholder; }
            set
            {
                _placeholder = value;
                OnPropertyChanged();
            }
        }

        private IEnumerable<object> _itemsSource;

        public IEnumerable<object> ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                _itemsSource = value;
                if (OriginItems == null && FilteredItems == null)
                {
                    OriginItems = value.ToList();
                }
                OnPropertyChanged();
            }
        }
        #endregion
        #region Search Functionality
        public List<object> OriginItems { get; set; }
        public List<object> FilteredItems { get; set; }
        private void comboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check for empty to reset ComboBox List
            if (string.IsNullOrEmpty(SearchText))
            {
                if (SelectedItem != null)
                    SelectedItem = null;
                FilteredItems = null;
                ItemsSource = OriginItems;
                Placeholder = "Search...";

                //show the dropdown to show the user all the options
                comboBox.IsDropDownOpen = true;
                return;
            }
            Placeholder = string.Empty;

            // Filter the list based on the search text and order by position
            FilteredItems = OriginItems
                .Where(item => item.ToString().ToLower().Contains(SearchText.ToLower()))
                .OrderBy(item => item.ToString().IndexOf(SearchText.ToLower()))
                .ToList();

            // Update the ComboBox with the new items that match the search text
            ItemsSource = FilteredItems;

            // hide or show drop down depedning on FilteredItems
            comboBox.IsDropDownOpen = FilteredItems.Count <= 0 ? false : true;
        }
        #endregion
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
