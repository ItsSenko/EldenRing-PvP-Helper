using Erd_Tools;
using PvPHelper.Core;
using PvPHelper.MVVM.Commands.Misc;

namespace PvPHelper.MVVM.ViewModels
{
    public class MiscViewModel : ViewModelBase
    {
        #region DataBindings
        public ShowHitboxesToggle ShowHitboxesToggle { get; set; }
        public AutoUpdateToggle AutoUpdateToggle { get; set; }
        public CheckForUpdates CheckForUpdates { get; set; }
        public UnsafeToggle UnsafeToggle { get; set; }
        #endregion
        private ErdHook hook;
        public MiscViewModel(ErdHook hook, VersionController versionController)
        {
            this.hook = hook;

            ShowHitboxesToggle = new(hook);
            AutoUpdateToggle = new();
            CheckForUpdates = new(versionController);
            UnsafeToggle = new();
        }
    }
}
