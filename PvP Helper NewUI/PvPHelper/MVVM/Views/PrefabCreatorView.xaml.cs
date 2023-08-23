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
        public List<string> Items { get; set; }
        public PrefabCreatorView()
        {
            InitializeComponent();
            DataContext = this;
            Items = new List<string>();

            // Replace this with your actual data source.
            for (int i = 1; i <= 60; i++)
            {
                Items.Add("Item " + i);
            }
        }


        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Initiates the drag-and-drop operation.
            isDragging = true;
            startPoint = e.GetPosition(null);
            draggedButton = sender as Button;
            initialIndex = Items.IndexOf(draggedButton.Content.ToString());
        }

        private void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            // Performs the drag operation if needed.
            if (isDragging && sender is Button button)
            {
                Point currentPosition = e.GetPosition(null);
                Vector diff = startPoint - currentPosition;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    (System.Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                     System.Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    // Start the drag-and-drop operation
                    DataObject dataObject = new DataObject("myFormat", button);
                    DragDrop.DoDragDrop(button, dataObject, DragDropEffects.Move);
                }
            }
        }

        private void Button_Drop(object sender, DragEventArgs e)
        {
            // Handles the drop operation
            if (sender is Button targetButton && e.Data.GetData("myFormat") is Button sourceButton)
            {
                int targetIndex = Items.IndexOf(targetButton.Content.ToString());
                int sourceIndex = initialIndex;
                Items.RemoveAt(sourceIndex);
                Items.Insert(targetIndex, sourceButton.Content.ToString());
            }
        }

        private void Button_DragEnter(object sender, DragEventArgs e)
        {
            // Sets the cursor and effect during drag-and-drop operation
            if (e.Data.GetDataPresent("myFormat") && sender is Button)
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

    }
}
