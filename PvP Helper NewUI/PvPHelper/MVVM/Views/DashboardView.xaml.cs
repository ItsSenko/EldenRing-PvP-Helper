using PvPHelper.MVVM.ViewModels;
using System;
using System.Windows.Controls;

namespace PvPHelper.MVVM.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public static event Action OnLoaded = new(() => { });
        public DashboardView()
        {
            InitializeComponent();
            this.Loaded += (s, e) => 
            {
                OnLoaded.Invoke();
            };
        }
    }
}
