using Erd_Tools;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
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
        private DispatcherTimer _timer;
        private NetPlayer localPlayer;

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
            _timer = new();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += CheckDebugFlag;

            PHPointer session = hook.CreateChildPointer(hook.WorldChrMan, 0x10EF8);

            localPlayer = new(hook, session, 0x0*10);
        }

        private void CheckDebugFlag(object? sender, EventArgs e)
        {
            if (!State || !_hook.Loaded)
                return;

            if (!GetConsumableFlag())
                SetConsumableFlag(true);

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

                byte b = row.Param.Pointer.ReadByte((int)dataOffset + consumeOffset);
                b = Helpers.SetBit(b, bitIndex, State ? false : true);

                row.Param.Pointer.WriteByte((int)dataOffset + consumeOffset, b);
            }

            CustomPointers.ChrDbgFlags.WriteByte(0x6, State ? (byte)1 : (byte)0);

            if (State)
                _timer.Start();
            else
                _timer.Stop();

            SetConsumableFlag(State);

            CommandManager.Log($"Infinite Consumables Toggled {State}.");
        }

        void SetConsumableFlag(bool state)
        {
            byte debugFlags = localPlayer.NetPlayerData.ReadByte(0x532);
            localPlayer.NetPlayerData.WriteByte(0x532, Helpers.SetBit(debugFlags, 0, state));
        }

        bool GetConsumableFlag()
        {
            byte debugFlags = localPlayer.NetPlayerData.ReadByte(0x532);
            return Helpers.IsBitSet(debugFlags, 0);
        }
    }
}
