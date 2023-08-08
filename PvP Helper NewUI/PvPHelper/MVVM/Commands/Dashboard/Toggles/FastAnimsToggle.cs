using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    internal class FastAnimsToggle : CommandBase, IToggleable
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
            if (!_hook.Hooked || !_hook.Loaded)
            {
                State = false;
                return;
            }

            var seamlessRuleBook = _hook.EquipParamGoods.Rows[8];
            var consumeOffset = seamlessRuleBook.Param.Fields[28].FieldOffset;
            var dataOffset = seamlessRuleBook.DataOffset;

            seamlessRuleBook.Param.Pointer.WriteByte(dataOffset + consumeOffset, State ? (byte)16 : (byte)60);

            CommandManager.Log($"Judicator's Rulebook animation changed to {(State ? "'Quick'" : "'Prayer'")} Animation.");
        }
    }
}
