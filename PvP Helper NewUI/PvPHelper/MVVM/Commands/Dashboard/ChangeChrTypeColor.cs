using Erd_Tools;
using PvPHelper.MVVM.Models;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard
{
    internal class ChangeChrTypeColor : CommandBase
    {
        private ErdHook _hook;
        public ChrType CurrChrType;
        public ChangeChrTypeColor(ErdHook hook)
        {
            _hook = hook;
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Loaded || !_hook.Hooked)
                return;

        }
    }
}
