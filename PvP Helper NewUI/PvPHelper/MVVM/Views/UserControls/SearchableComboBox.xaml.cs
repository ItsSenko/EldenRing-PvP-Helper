using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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


        public ICommand ShiftUpCommand
        {
            get { return (ICommand)GetValue(ShiftUpCommandProperty); }
            set { SetValue(ShiftUpCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShiftUpCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShiftUpCommandProperty =
            DependencyProperty.Register("ShiftUpCommand", typeof(ICommand), typeof(SearchableComboBox));


        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set 
            { 
                SetValue(SelectedItemProperty, value); 
                OnSelectedItemChanged.Invoke(value); 
            }
        }
        public event Action<object> OnSelectedItemChanged = new((obj) => { });
        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(SearchableComboBox));



        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SearchableComboBox));



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
                if (OriginItems == null && value != null)
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
        /*public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var element = GetTemplateChild("comboBox");
            if (element != null)
            {
                var textBox = Helpers.FindVisualChild<TextBox>(element);

                if (textBox!= null)
                {
                    textBox.SelectionChanged += OnDropSelectionChanged;
                }
            }
        }

        bool dropDownOpened = false;
        private void OnDropSelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;

            if (dropDownOpened)
            {
                box.SelectionLength = 0;
            }
        }*/
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
                return;
            }
            Placeholder = string.Empty;

            if (SelectedItem != null)
                return;

            if (ItemsSource == null || ItemsSource.ToList().Count == 0)
                return;

            ItemsSource = OriginItems.Where(item => item.ToString().ToLower().Contains(SearchText.ToLower()));

            bool doDropDown = ItemsSource.ToList().Count <= 0 ? false : true && !string.IsNullOrEmpty(SearchText);

            if (comboBox.IsDropDownOpen != doDropDown)
            {
                comboBox.IsManuallyDroppedDown = doDropDown;
                comboBox.IsDropDownOpen = doDropDown;
            }

            /*if (SelectedItem != null)
                return;
            // Check if there is any items in the ItemsSource
            if (ItemsSource == null || ItemsSource.ToList().Count == 0) return;

            // Filter the list based on the search text and order by position
            FilteredItems = OriginItems.Where(item => item.ToString().ToLower().Contains(SearchText.ToLower())).OrderBy(item => item.ToString().IndexOf(SearchText.ToLower())).ToList();

            // Update the ComboBox with the new items that match the search text
            ItemsSource = FilteredItems;

            // hide or show drop down depedning on FilteredItems
            comboBox.IsDropDownOpen = FilteredItems.Count <= 0 ? false : true;*/
        }
        #endregion

        private TextBox box;
        private void comboBox_OnTextBoxSet(TextBox obj)
        {
            box = obj;
        }
    }
}
