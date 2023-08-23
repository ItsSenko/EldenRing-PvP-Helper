using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for NumberInput.xaml
    /// </summary>
    public partial class NumberInput : UserControl
    {
        public NumberInput()
        {
            InitializeComponent();
        }
        #region Data Bindings


        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(NumberInput), new PropertyMetadata(string.Empty));



        public int CurrValue
        {
            get { return (int)GetValue(CurrValueProperty); }
            set { SetValue(CurrValueProperty, value); UpdateText(); }
        }

        // Using a DependencyProperty as the backing store for CurrValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrValueProperty =
            DependencyProperty.Register("CurrValue", typeof(int), typeof(NumberInput), new PropertyMetadata(0));



        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(NumberInput), new PropertyMetadata(0));



        public int Min
        {
            get { return (int)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(int), typeof(NumberInput), new PropertyMetadata(0));

        #endregion
        private bool IsTextAllowed(string text)
        {
            int parsed;
            if (!int.TryParse(InputText + text, out parsed))
                return false;

            if (parsed < Min)
                return false;
            if (parsed > Max)
                return false;

            return true;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(InputText, out int parsed))
                CurrValue = parsed;
        }
        private void Minus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrValue > Min)
                CurrValue--;
        }
        private void UpdateText()
        {
            InputText = CurrValue.ToString();
        }
        private void Plus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrValue < Max)
                CurrValue++;
        }
    }
}
