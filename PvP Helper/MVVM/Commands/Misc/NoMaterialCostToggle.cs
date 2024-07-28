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

namespace PvPHelper.MVVM.Commands.Misc
{
    public class NoMaterialCostToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }

        private ErdHook Hook;
        private Param MtrlParam;
        public NoMaterialCostToggle(ErdHook hook)
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

            if (MtrlParam == null)
                MtrlParam = Hook.Params.FirstOrDefault(x => x.Name == "EquipMtrlSetParam");

            if (!State)
            {
                MtrlParam.RestoreParam();
                return;
            }

            var materialIdOffset = MtrlParam.Fields.FirstOrDefault(x => x.InternalName == "materialId01").FieldOffset;
            var itemNumOffset = MtrlParam.Fields.FirstOrDefault(x => x.InternalName == "itemNum01").FieldOffset;

            foreach (var row in MtrlParam.Rows)
            {
                MtrlParam.Pointer.WriteInt32((int)row.DataOffset + materialIdOffset, (sbyte)-1);
                MtrlParam.Pointer.WriteSByte((int)row.DataOffset + itemNumOffset, (sbyte)-1);
            }
        }
    }
}
