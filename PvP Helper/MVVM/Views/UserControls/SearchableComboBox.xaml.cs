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

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
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

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchableComboBox), new PropertyMetadata(string.Empty));



        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(SearchableComboBox), new PropertyMetadata(string.Empty));



        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(SearchableComboBox));
        #endregion
        private void comboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchText))
                Placeholder = string.Empty;
            else
                Placeholder = "Search...";
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!comboBox.IsDropDownOpen)
            {
                var textBox = (TextBox)this.Template.FindName("PART_EditableTextBox", this);
                if (textBox != null)
                {
                    textBox.SelectionLength = 0;
                }
            }
            else if (SearchText != null && SearchText.Length < 2)
            {
                var textBox = (TextBox)this.Template.FindName("PART_EditableTextBox", this);
                if (textBox != null)
                {
                    textBox.SelectionLength = 0;
                }
            }
        }

        private void comboBox_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)comboBox.Template.FindName("PART_EditableTextBox", comboBox);
            if (!comboBox.IsDropDownOpen && string.IsNullOrEmpty(SearchText) && textBox.IsFocused)
            {
                comboBox.IsDropDownOpen = true;
                if (textBox != null)
                {
                    textBox.SelectionLength = 0;
                }
            }

            if (textBox != null && textBox.SelectionLength < 1)
            {
                textBox.SelectionLength = 0;
            }
        }
    }
}
