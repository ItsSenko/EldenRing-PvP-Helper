using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using PvPHelper.Console;
using System.Windows.Input;
using CommandManager = PvPHelper.Console.CommandManager;

namespace PvPHelper.Core.Hotkeys
{
    public class Hotkeys
    {
        private static Hotkeys _instance;

        public static Hotkeys Instance
        {
            get 
            {
                if (_instance == null)
                    _instance = new Hotkeys();

                return _instance; 
            }
            set { _instance = value; }
        }

        private string HotkeyJsonPath => Path.Combine(Directory.GetCurrentDirectory(), "Resources/HotkeySettings.json");
        private string Json { get; set; }
        private GlobalHotKey.HotKeyManager HotKeyManager { get; set; }
        private List<GlobalHotKey.HotKey> RegisteredKeys = new();
        public SavedHotkeys SavedHotkeys { get; set; }
        public Hotkeys()
        {
            if (!File.Exists(HotkeyJsonPath))
            {
                var stream = File.Create(HotkeyJsonPath);
                stream.Close();
            }
            SavedHotkeys = new SavedHotkeys(new());

            Json = File.ReadAllText(HotkeyJsonPath);

            HotKeyManager = new();

            HotKeyManager.KeyPressed += HotKeyManager_KeyPressed;

            LoadSavedHotkeys();
        }

        private void HotKeyManager_KeyPressed(object? sender, GlobalHotKey.KeyPressedEventArgs e)
        {
            Hotkey match = SavedHotkeys.Hotkeys.FirstOrDefault(x => x.HotKey.IsEquals(e.HotKey));

            if (match == null)
                return;

            match.Invoke();
        }

        public Hotkey GetSavedHotKey(string name, GlobalHotKey.HotKey key = null)
        {
            Hotkey returnKey = SavedHotkeys.Hotkeys.FirstOrDefault(x => x.Name == name);

            if (returnKey == null)
            {
                returnKey = new(name, key);
                SavedHotkeys.Hotkeys.Add(returnKey);
                SaveHotkeys();
                RegisterAll();
            }

            return returnKey;
        }

        private void LoadSavedHotkeys()
        {
            if (string.IsNullOrEmpty(Json))
                SaveHotkeys();

            SavedHotkeys = JsonConvert.DeserializeObject<SavedHotkeys>(Json);

            if (SavedHotkeys == null)
                throw new Exception("Hotkeys failed to load.");

            if (SavedHotkeys.Hotkeys == null || SavedHotkeys.Hotkeys.Count == 0)
            {
                SavedHotkeys.Hotkeys = new();
                SavedHotkeys.Hotkeys.Add(new("Example", new(Key.NumPad9, ModifierKeys.None)));
                SaveHotkeys();
            }

            RegisterAll();
        }

        private void SaveHotkeys()
        {
            Json = JsonConvert.SerializeObject(SavedHotkeys, Formatting.Indented);

            File.WriteAllText(HotkeyJsonPath, Json);
        }

        private void RegisterAll()
        {
            foreach(GlobalHotKey.HotKey key in RegisteredKeys)
            {
                HotKeyManager.Unregister(key);
            }
            RegisteredKeys.Clear();

            foreach (Hotkey key in SavedHotkeys.Hotkeys)
            {
                if (!RegisteredKeys.Contains(key.HotKey))
                {
                    HotKeyManager.Register(key.HotKey);
                    RegisteredKeys.Add(key.HotKey);
                }
            }
        }
    }


}
