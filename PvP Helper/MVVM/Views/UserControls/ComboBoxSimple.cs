using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PvPHelper.MVVM.Views.UserControls
{
    public class ComboBoxSimple : ComboBox
    {
        private int caretPosition;
        public bool IsManuallyDroppedDown = false;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var element = GetTemplateChild("PART_EditableTextBox");
            if (element != null)
            {
                var textBox = (TextBox)element;
                textBox.SelectionChanged += OnDropSelectionChanged;
                TextBox = textBox;
            }
        }
        public event Action<TextBox> OnTextBoxSet = new((t) => { });
        private TextBox _textBox;

        public TextBox TextBox
        {
            get { return _textBox; }
            set { _textBox = value; OnTextBoxSet.Invoke(value); }
        }

        private void OnDropSelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (IsManuallyDroppedDown && txt.SelectionLength > 0)
            {
                caretPosition = txt.SelectionLength; // caretPosition must be set to TextBox's SelectionLength
                txt.CaretIndex = caretPosition;
                IsManuallyDroppedDown = false;
            }
            /*if (base.IsDropDownOpen && txt.SelectionLength > 0)
            {
                caretPosition = txt.SelectionLength; // caretPosition must be set to TextBox's SelectionLength
                txt.CaretIndex = caretPosition;
            }
            if (txt.SelectionLength == 0 && txt.CaretIndex != 0)
            {
                caretPosition = txt.CaretIndex;
            }*/
        }
    }
}
