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
    }
}
