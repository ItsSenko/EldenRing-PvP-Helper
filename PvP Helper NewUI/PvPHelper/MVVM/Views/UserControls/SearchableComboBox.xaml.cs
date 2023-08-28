using PvPHelper.MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SearchableComboBox.xaml
    /// </summary>
    public partial class SearchableComboBox : UserControl
    {
        public SearchableComboBox()
        {
            InitializeComponent();
            Placeholder = "Search...";
        }
        #region DataBindings
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(SearchableComboBox));



        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchableComboBox), new PropertyMetadata(string.Empty));



        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(SearchableComboBox), new PropertyMetadata(string.Empty));



        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set 
            {
                if (OriginItems == null && FilteredItems == null && value != null)
                {
                    OriginItems = value.ToList();
                }
                SetValue(ItemsSourceProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(SearchableComboBox));
        #endregion
        #region Search Functionality


        public List<object> OriginItems
        {
            get { return (List<object>)GetValue(OriginItemsProperty); }
            set { SetValue(OriginItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OriginItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OriginItemsProperty =
            DependencyProperty.Register("OriginItems", typeof(List<object>), typeof(SearchableComboBox));


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
            
            if (SelectedItem != null)
                return;
            // Check if there is any items in the ItemsSource
            if (ItemsSource == null || ItemsSource.ToList().Count == 0) return;

            // Filter the list based on the search text and order by position
            FilteredItems = OriginItems.Where(item => item.ToString().ToLower().Contains(SearchText.ToLower())).OrderBy(item => item.ToString().IndexOf(SearchText.ToLower())).ToList();

            // Update the ComboBox with the new items that match the search text
            ItemsSource = FilteredItems;

            // hide or show drop down depedning on FilteredItems
            comboBox.IsDropDownOpen = FilteredItems.Count <= 0 ? false : true;
        }
        #endregion
    }
}
