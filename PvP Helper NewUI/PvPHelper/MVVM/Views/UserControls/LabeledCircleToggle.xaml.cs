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
    /// Interaction logic for LabeledCircleToggle.xaml
    /// </summary>
    public partial class LabeledCircleToggle : UserControl
    {
        public LabeledCircleToggle()
        {
            InitializeComponent();
        }


        public bool Checked
        {
            get { return (bool)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Checked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckedProperty =
            DependencyProperty.Register("Checked", typeof(bool), typeof(LabeledCircleToggle), new PropertyMetadata(false));


        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Labal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(LabeledCircleToggle), new PropertyMetadata(string.Empty));


        public ICommand ToggleCommand
        {
            get { return (ICommand)GetValue(ToggleCommandProperty); }
            set { SetValue(ToggleCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleCommandProperty =
            DependencyProperty.Register("ToggleCommand", typeof(ICommand), typeof(LabeledCircleToggle));


    }
}
