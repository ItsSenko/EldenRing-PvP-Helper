using Erd_Tools;
using PropertyHook;
using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Commands.Misc
{
    public class CustomFPSToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        private ErdHook Hook;
        public CustomFPSToggle(ErdHook hook)
        {
            Hook = hook;
        }
        public override void Execute(object? parameter)
        {
            if (!Hook.Loaded || !Hook.Hooked)
            {
                State = false;
                return;
            }

            if (!State)
            {
                CustomPointers.CSFlipper.WriteSingle(0x2CC, 60);
                CustomPointers.CSFlipper.WriteByte(0x2D0, 00);
                return;
            }

            InputDialog dialog = new("Input FPS Value...");
            dialog.OnSave += OnInputValue;
            dialog.OnCancel += () => { State = false; };
            dialog.ShowDialog();
        }

        private void OnInputValue(string value)
        {
            if (!int.TryParse(value, out var newfps))
            {
                InformationDialog dialog = new("Invalid Value. Please input only Int type values. Ex: 60");
                State = false;
                dialog.ShowDialog();
                return;
            }

            if (newfps > 999 || newfps < 1)
            {
                InformationDialog dialog = new($"Input Value is either too high or too low. This will make the game unplayable. Your Value: {newfps}");
                State = false;
                return;
            }

            CustomPointers.CSFlipper.WriteSingle(0x2CC, newfps);
            CustomPointers.CSFlipper.WriteByte(0x2D0, 01);
        }
    }
}
