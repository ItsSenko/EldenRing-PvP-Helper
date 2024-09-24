using GlobalHotKey;

namespace PvPHelper.Core.Hotkeys
{
    public static class HotkeyExtensions
    {
        public static bool IsEquals(this HotKey hk1, HotKey hk2)
        {
            return hk1.Key == hk2.Key && hk1.Modifiers == hk2.Modifiers;
        }
    }
}
