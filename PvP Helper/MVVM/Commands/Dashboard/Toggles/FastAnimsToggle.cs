using Erd_Tools;
using PvPHelper.Console;
using PvPHelper.Core;
using System.Linq;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    public class FastAnimsToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook _hook;

        public FastAnimsToggle(ErdHook hook)
        {
            _hook = hook;
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Hooked || !_hook.Loaded || !Helpers.GetIfModuleExists(_hook.Process, "ersc.dll"))
            {
                State = false;
                return;
            }

            var seamlessRuleBook = _hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == (int)Helpers.SeamlessItems.GameRuleChangeItem);
            var hostItem = _hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == (int)Helpers.SeamlessItems.HostingItem);
            var joinItem = _hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == (int)Helpers.SeamlessItems.JoiningItem);
            var consumeOffset = seamlessRuleBook.Param.Fields[28].FieldOffset;
            

            seamlessRuleBook.Param.Pointer.WriteByte(hostItem.DataOffset + consumeOffset, State ? (byte)16 : (byte)6);
            seamlessRuleBook.Param.Pointer.WriteByte(seamlessRuleBook.DataOffset + consumeOffset, State ? (byte)16 : (byte)60);
            seamlessRuleBook.Param.Pointer.WriteByte(joinItem.DataOffset + consumeOffset, State ? (byte)16 : (byte)41);


            CommandManager.Log($"Hosting, Joining, and RuleBook, Seamless Items are now {(State ? "Fast" : "Normal")}");
        }
    }
}
