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
    internal class InfiniteConsumeablesToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook _hook;
        private List<int> consumableIds = new();

        public InfiniteConsumeablesToggle(ErdHook hook)
        {
            _hook = hook;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "PvPHelper.Resources.ConsumableIDS.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                consumableIds = Helpers.EnumerateLines(reader).Select(x => Convert.ToInt32(x)).ToList();
            }
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Hooked || !_hook.Loaded)
            {
                State = false;
                return;
            }

            CommandManager.Log($"Changing consumbables 'isConsume' param to {(State ? "0" : "1")}..");

            var bitIndex = 7;
            var consumeOffset = _hook.EquipParamGoods.Fields[40].FieldOffset;

            for (int i = 0; i < consumableIds.Count - 1; i++)
            {
                var row = _hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == consumableIds[i]);
                var dataOffset = row.DataOffset;

                byte b = row.Param.Pointer.ReadByte(dataOffset + consumeOffset);
                b = Helpers.SetBit(b, bitIndex, State ? false : true);

                row.Param.Pointer.WriteByte(dataOffset + consumeOffset, b);
            }

            CommandManager.Log($"Infinite Consumables Toggled {State}.");
        }
    }
}
