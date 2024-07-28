using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PvPHelper.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Credit.xaml
    /// </summary>
    public partial class Credit : UserControl
    {
        public Credit()
        {
            InitializeComponent();
        }



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Credit));



        public string Details
        {
            get { return (string)GetValue(DetailsProperty); }
            set { SetValue(DetailsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Details.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetailsProperty =
            DependencyProperty.Register("Details", typeof(string), typeof(Credit));



        public Brush TitleBrush
        {
            get { return (Brush)GetValue(TitleBrushProperty); }
            set { SetValue(TitleBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NameBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleBrushProperty =
            DependencyProperty.Register("TitleBrush", typeof(Brush), typeof(Credit));



        public string Link
        {
            get { return (string)GetValue(LinkProperty); }
            set { SetValue(LinkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Link.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LinkProperty =
            DependencyProperty.Register("Link", typeof(string), typeof(Credit));



        public ICommand OnClick
        {
            get { return (ICommand)GetValue(OnClickProperty); }
            set { SetValue(OnClickProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OnClick.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnClickProperty =
            DependencyProperty.Register("OnClick", typeof(ICommand), typeof(Credit));



        public Cursor CursorType
        {
            get { return (Cursor)GetValue(CursorTypeProperty); }
            set { SetValue(CursorTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CursorType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CursorTypeProperty =
            DependencyProperty.Register("CursorType", typeof(Cursor), typeof(Credit));



        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnClick?.Execute(null);
        }
    }
}
