using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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



        public string MinusImgSource
        {
            get { return (string)GetValue(MinusImgSourceProperty); }
            set { SetValue(MinusImgSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinusImgSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinusImgSourceProperty =
            DependencyProperty.Register("MinusImgSource", typeof(string), typeof(NumberInput));



        public string PlusImgSource
        {
            get { return (string)GetValue(PlusImgSourceProperty); }
            set { SetValue(PlusImgSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlusImgSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlusImgSourceProperty =
            DependencyProperty.Register("PlusImgSource", typeof(string), typeof(NumberInput));


        #endregion
        private bool IsTextAllowed(string text)
        {
            int parsed;
            if (MainTextBox.SelectionLength == InputText.Length)
            {
                if (!int.TryParse(text, out parsed))
                    return false;

                if (parsed < Min)
                    return false;
                if (parsed > Max)
                    return false;

                return true;
            }
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

        private void Minus_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Minus.Fill = (Brush)bc.ConvertFrom("#737373");
        }

        private void Minus_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Minus.Fill = (Brush)bc.ConvertFrom("#A953FF"); ;
        }

        private void Plus_MouseEnter(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Plus.Fill = (Brush)bc.ConvertFrom("#737373");
        }

        private void Plus_MouseLeave(object sender, MouseEventArgs e)
        {
            var bc = new BrushConverter();
            Plus.Fill = (Brush)bc.ConvertFrom("#A953FF");
        }
    }
}
