using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using PvPHelper.Core;
using System.Windows.Input;
using System.Diagnostics;

namespace PvPHelper.MVVM.Models
{
    public class CreditModel
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string Color { get; set; }
        public string Link { get; set; }

        [JsonIgnore()]
        public Brush TitleBrush { get; set; }

        [JsonIgnore()]
        public Cursor CursorType { get; set; }

        [JsonIgnore()]
        public RelayCommand OnClick { get; set; }

        public CreditModel(string title, string details, string color, string link)
        {
            Title = title;
            Details = details;
            Color = color;
            Link = link;
        }

        public void Initialize()
        {
            TitleBrush = Helpers.GetColor(Color);
            CursorType = !string.IsNullOrEmpty(Link) ? Cursors.Hand : null;

            OnClick = new(o => OnClicked());
        }

        private void OnClicked()
        {
            if (string.IsNullOrEmpty(Link))
                return;

            try
            {
                var ps = new ProcessStartInfo(Link)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            }
            catch { }
        }
    }
}
