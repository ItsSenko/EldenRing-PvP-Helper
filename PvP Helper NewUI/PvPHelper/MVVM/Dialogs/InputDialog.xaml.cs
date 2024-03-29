﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PvPHelper.MVVM.Dialogs
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public event Action OnCancel = new(() => { });
        public event Action<string> OnSave = new((s) => { });
        public string MainTitle;
        public InputDialog(string title)
        {
            InitializeComponent();
            MainTitle = title;
            TextBox.Text = MainTitle;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnSave.Invoke(TextBox.Text);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OnCancel.Invoke();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBox.Text != MainTitle)
                return;

            TextBox.Text = string.Empty;
        }
    }
}
