using Erd_Tools;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard
{
    internal class AttachCommand : CommandBase
    {
        private ErdHook _hook;

        public AttachCommand(ErdHook hook)
        {
            _hook = hook;
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Hooked)
            {
                _hook.Start();
            }
        }
    }
}
