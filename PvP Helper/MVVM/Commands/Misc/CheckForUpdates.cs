using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using System.Threading.Tasks;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class CheckForUpdates : CommandBase
    {
        public VersionController _vController;
        public CheckForUpdates(VersionController versionController)
        {
            _vController = versionController;
        }
        public override void Execute(object? parameter)
        {
            if (_vController.UpdateAvailable)
            {
                InformationDialog dialog = new($"Update {_vController.CurrentVersion} is available. Do you want to update?");
                dialog.OnOk += () =>
                {
                    _vController.Update(_vController._releaseUrl, _vController.CurrentVersion);
                };
                dialog.ShowDialog();
            }
            else
            {
                InformationDialog dialog = new($"There are currently no updates available.");
                dialog.ShowDialog();
            }
        }
    }
}
