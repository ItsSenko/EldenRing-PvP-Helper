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
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using PvPHelper.Core;

namespace PvPHelper.MVVM.Dialogs
{
    /// <summary>
    /// Interaction logic for ProgressBarDialog.xaml
    /// </summary>
    public partial class ProgressBarDialog : Window
    {
        public ProgressBarDialog(string title)
        {
            InitializeComponent();

            TitleBox.Text = title;
        }
        

        public void StartTask(Thread thread)
        {
            thread.Start();
            ShowDialog();
        }

        public void Complete()
        {
            this.Close();
        }
    }
}
