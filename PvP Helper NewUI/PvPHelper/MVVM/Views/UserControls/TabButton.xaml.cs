﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TabButton.xaml
    /// </summary>
    public partial class TabButton : UserControl
    {
        public string UriSource
        {
            get { return (string)GetValue(UriSourceProperty); }
            set { SetValue(UriSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UriSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UriSourceProperty =
            DependencyProperty.Register("UriSource", typeof(string), typeof(TabButton), new PropertyMetadata(string.Empty));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(TabButton), new PropertyMetadata(string.Empty));

        public ICommand BtnCommand
        {
            get { return (ICommand)GetValue(BtnCommandProperty); }
            set { SetValue(BtnCommandProperty, value); }   
        }

        public static readonly DependencyProperty BtnCommandProperty =
            DependencyProperty.Register("BtnCommand", typeof(ICommand), typeof(TabButton));

        public TabButton()
        {
            InitializeComponent();
        }
    }
}
