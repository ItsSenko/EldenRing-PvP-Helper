using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for NumberInput.xaml
    /// </summary>
    public partial class NumberInput : UserControl, INotifyPropertyChanged
    {
        public NumberInput()
        {
            DataContext = this;
            InitializeComponent();
        }
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
        #region Data Bindings
        private string _inputText;

        public string InputText
        {
            get { return _inputText; }
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }
        private int _currValue;

        public int CurrValue
        {
            get { return _currValue; }
            set
            {
                _currValue = value;
                InputText = CurrValue.ToString();
                OnPropertyChanged();
            }
        }

        private int _max;

        public int Max
        {
            get { return _max; }
            set
            {
                _max = value;
                OnPropertyChanged();
            }
        }

        private int _min;

        public int Min
        {
            get { return _min; }
            set
            {
                _min = value;
                OnPropertyChanged();
            }
        }

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

            if (!e.Handled && int.TryParse(InputText, out int parsed))
                CurrValue = parsed;
        }

        private void Minus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrValue > Min)
                CurrValue--;
        }

        private void Plus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrValue < Max)
                CurrValue++;
        }
    }
}
