﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SimpleDropDown.xaml
    /// </summary>
    public partial class SimpleDropDown : UserControl
    {
        public SimpleDropDown()
        {
            InitializeComponent();
        }
        #region Data Bindings




        public bool Border
        {
            get { return (bool)GetValue(BorderProperty); }
            set { SetValue(BorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Border.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderProperty =
            DependencyProperty.Register("Border", typeof(bool), typeof(SimpleDropDown));




        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(SimpleDropDown));


        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SimpleDropDown), new PropertyMetadata(0));
        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set 
            { 
                SetValue(ItemsSourceProperty, value);
                if (value == null)
                {
                    GhostTextBlock.Visibility = comboBox.IsDropDownOpen ? Visibility.Hidden : Visibility.Visible;
                }
                else
                    GhostTextBlock.Visibility = Visibility.Visible;
            }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(SimpleDropDown));





        public string GhostText
        {
            get { return (string)GetValue(GhostTextProperty); }
            set { SetValue(GhostTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GhostText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GhostTextProperty =
            DependencyProperty.Register("GhostText", typeof(string), typeof(SimpleDropDown));




        #endregion

        private void ComboBox_DropDownOpened(object sender, System.EventArgs e)
        {
            GhostTextBlock.Visibility = Visibility.Hidden;
        }

        private void ComboBox_DropDownClosed(object sender, System.EventArgs e)
        {
            if (SelectedItem == null)
                GhostTextBlock.Visibility = Visibility.Visible;
        }
    }
}
