﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Refresh.xaml
    /// </summary>
    public partial class Refresh : UserControl
    {
        public Refresh()
        {
            InitializeComponent();
        }
        BrushConverter bc = new();


        public ICommand BtnCommand
        {
            get { return (ICommand)GetValue(BtnCommandProperty); }
            set { SetValue(BtnCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BtnCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BtnCommandProperty =
            DependencyProperty.Register("BtnCommand", typeof(ICommand), typeof(Refresh));

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Icon.Fill = (Brush)bc.ConvertFrom("#737373");
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Icon.Fill = Brushes.White;
        }
    }
}
