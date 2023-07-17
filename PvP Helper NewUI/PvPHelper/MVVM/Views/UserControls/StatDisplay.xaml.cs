using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for StatDisplay.xaml
    /// </summary>
    public partial class StatDisplay : UserControl,INotifyPropertyChanged
    {
        public static readonly DependencyProperty fill =
                   DependencyProperty.Register(
                         "Fill",
                          typeof(Brush),
                          typeof(StatDisplay));
        public Brush Fill
        {
            get
            {
                return (Brush)GetValue(fill);
            }
            set
            {
                SetValue(fill, value);
            }
        }

        private string uriSource = "Resources/file-document-alert.svg";
        public string UriSource
        {
            get { return uriSource; }
            set { uriSource = value; OnPropertyChanged(); }
        }
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value;  OnPropertyChanged(); }
        }
        private int _min;

        public int Min
        {
            get { return _min; }
            set { _min = value; OnPropertyChanged(); }
        }
        private int _max;

        public int Max
        {
            get { return _max; }
            set { _max = value; OnPropertyChanged(); }
        }
        private int _currValue;

        public int CurrValue
        {
            get { return _currValue; }
            set { _currValue = value; Text = CurrValue.ToString(); OnPropertyChanged(); }
        }


        public StatDisplay()
        {
            DataContext = this;
            InitializeComponent();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private bool IsTextAllowed(string text)
        {
            int parsed;
            if (!int.TryParse(Text + text, out parsed))
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

            if (!e.Handled && int.TryParse(Text, out int parsed))
                CurrValue = parsed;
        }
    }
}
