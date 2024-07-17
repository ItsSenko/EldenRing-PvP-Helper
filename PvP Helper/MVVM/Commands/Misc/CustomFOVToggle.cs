using Erd_Tools;
using Erd_Tools.Models;
using PropertyHook;
using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Erd_Tools.Models.Param;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class CustomFOVToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        private ErdHook Hook;
        private Param LockCamParam;
        public CustomFOVToggle(ErdHook hook)
        {
            Hook = hook;
        }
        public override void Execute(object? parameter)
        {
            if (!Hook.Hooked || !Hook.Loaded)
            {
                State = false;
                return;
            }

            if (LockCamParam == null)
                LockCamParam = Hook.Params.FirstOrDefault(x => x.Name == "LockCamParam");

            if (!State)
            {
                LockCamParam.RestoreParam();
                return;
            }

            InputDialog dialog = new("Input FOV Value...");
            dialog.OnSave += OnInputValue;
            dialog.OnCancel += () => { State = false; };
            dialog.ShowDialog();
        }

        private void OnInputValue(string value)
        {
            if (!int.TryParse(value, out var newFov))
            {
                InformationDialog dialog = new("Invalid Value. Please input only Int type values. Ex: 60");
                State = false;
                dialog.ShowDialog();
                return;
            }

            if (newFov > 156 || newFov < 5)
            {
                InformationDialog dialog = new($"Input Value is either too high or too low. This will make the game unplayable. Your value: {newFov}");
                State = false;
                dialog.ShowDialog();
                return;
            }

            var offset = LockCamParam.Fields.FirstOrDefault(x => x.InternalName == "camFovY").FieldOffset;
            foreach (Row row in LockCamParam.Rows)
            {
                LockCamParam.Pointer.WriteSingle((int)row.DataOffset + offset, newFov);
            }
        }
    }
}
