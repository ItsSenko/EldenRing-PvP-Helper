using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalHotKey;

namespace PvPHelper.Core.Hotkeys
{
    public class SavedHotkeys
    {
        public List<Hotkey> Hotkeys { get; set; }

        public SavedHotkeys(List<Hotkey> hotkeys)
        {
            Hotkeys = hotkeys;
        }
    }
    public class Hotkey
    {
        public string Name { get; set; }
        public HotKey HotKey { get; set; }

        public delegate void OnPressedHandler();
        public event OnPressedHandler OnPressed;

        public Hotkey(string name, HotKey hotKey)
        {
            Name = name;
            HotKey = hotKey;
        }

        public void Invoke()
        {
            OnPressed?.Invoke();
        }
    }
}
