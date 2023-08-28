using PvPHelper.MVVM.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PvPHelper.MVVM.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class PrefabCreatorView : UserControl
    {
        private bool isDragging;
        private Button draggedButton;
        private Point startPoint;
        private int initialIndex;
        public PrefabCreatorView()
        {
            InitializeComponent();
        }

        private void Items_Drop(object sender, DragEventArgs e)
        {
            var droppedData = e.Data.GetData(typeof(InventoryItem)) as InventoryItem;
            var target = ((InventoryItem)(sender)).DataContext as InventoryItem;

            int removedIdx = Items.Items.IndexOf(droppedData);
            int targetIdx = Items.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                Items.Items.Insert(targetIdx + 1, droppedData);
                Items.Items.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (Items.Items.Count + 1 > remIdx)
                {
                    Items.Items.Insert(targetIdx, droppedData);
                    Items.Items.RemoveAt(remIdx);
                }
            }

            Items.Items.Refresh();
        }
        private void InventoryItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                DragDrop.DoDragDrop(item, new DataObject(DataFormats.Serializable, item.DataContext), DragDropEffects.Move);
            }
        }
    }
}
