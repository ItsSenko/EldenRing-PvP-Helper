using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERPvPHelper
{
    public class Logger
    {
        private RichTextBox LogsBox { get; set; }
        private StringBuilder sb { get; set; }
        private StringWriter sw { get; set; }
        private Form form { get; set; }
        public Logger(Form form, RichTextBox LogsBox, StringBuilder sb)
        {
            this.LogsBox = LogsBox;
            this.sb = sb;
            this.sw = new StringWriter(sb);
            this.form = form;

            this.LogsBox.TextChanged += (s, e) => 
            {
                this.LogsBox.SelectionStart = this.LogsBox.Text.Length;
                this.LogsBox.ScrollToCaret();
            };

            this.LogsBox.ScrollBars = RichTextBoxScrollBars.None;
        }
        public void Log(string str, LogType type = LogType.Normal)
        {
            form.Invoke(new Action(() =>
            {
                if (str.Length > 79)
                {
                    Log(str.Substring(0, 79));
                    Log(str.Substring(79));
                    return;
                }
                string SystemTime = DateTime.Now.ToString("[" + "h:mm:ss" + "]");
                sw.WriteLine($"{SystemTime} [PvPHelper] {str}");
                LogsBox.SelectionStart = LogsBox.TextLength;
                LogsBox.SelectionLength = 0;

                LogsBox.SelectionColor = Color.DarkMagenta;
                LogsBox.AppendText($"\n{SystemTime} [PvP");
                LogsBox.SelectionColor = Color.Magenta;
                LogsBox.AppendText($"Helper] ");
                LogsBox.SelectionColor = getLogTypeColor(type);
                LogsBox.AppendText($"{str}");
                LogsBox.SelectionColor = LogsBox.ForeColor;
            }));
        }
        public enum LogType
        {
            Normal,
            Warning,
            Error,
            Success
        }
        private Color getLogTypeColor(LogType type)
        {
            switch(type)
            {
                case LogType.Normal: return Color.DarkGray;
                case LogType.Warning: return Color.Yellow;
                case LogType.Error: return Color.Red;
                case LogType.Success: return Color.LightGreen;
                default: return Color.DarkGray;
            }
        }
        public void LogEmpty(string str)
        {
            form.Invoke(new Action(() =>
            {
                LogsBox.SelectionColor = Color.DarkGray;
                LogsBox.AppendText($"\n{str}");
                LogsBox.SelectionColor = LogsBox.ForeColor;
            }));
        }
    }
}
