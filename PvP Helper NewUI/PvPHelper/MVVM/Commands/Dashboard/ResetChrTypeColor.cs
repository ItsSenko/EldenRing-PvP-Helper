using Erd_Tools;
using Erd_Tools.Models;
using PvPHelper.Console;
using PvPHelper.MVVM.Dialogs;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Drawing;
using System.Linq;
using static Erd_Tools.Models.Param;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard
{
    internal class ResetChrTypeColor : CommandBase
    {
        public ErdHook _hook;
        public DashboardViewModel _dashboard;

        public Param PhantomParam => _hook.Params.FirstOrDefault(x => x.Name == "PhantomParam");
        public int edgeColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorR").FieldOffset;
        public int edgeColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorG").FieldOffset;
        public int edgeColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorB").FieldOffset;
        public int diffMulColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorR").FieldOffset;
        public int diffMulColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorG").FieldOffset;
        public int diffMulColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorB").FieldOffset;
        public int frontColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorR").FieldOffset;
        public int frontColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorG").FieldOffset;
        public int frontColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorB").FieldOffset;
        public ResetChrTypeColor(ErdHook hook, DashboardViewModel viewModel)
        {
            _hook = hook;
            _dashboard = viewModel;
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Loaded || !_hook.Hooked)
                return;

            var chrType = _dashboard.ChrTypeItemsSource.ToArray()[_dashboard.ChrTypeSelectedIndex] as ChrType;

            if (chrType.ChrID == 0)
                return;

            var phantomID = chrType.ParamID;
            Row row = PhantomParam.Rows.FirstOrDefault(x => x.ID == phantomID);
            var dataOffset = row.DataOffset;

            row.Param.Pointer.WriteByte(dataOffset + edgeColorR, chrType.edgeColor.R);
            row.Param.Pointer.WriteByte(dataOffset + edgeColorG, chrType.edgeColor.G);
            row.Param.Pointer.WriteByte(dataOffset + edgeColorB, chrType.edgeColor.B);

            row.Param.Pointer.WriteByte(dataOffset + diffMulColorR, chrType.diffMulColor.R);
            row.Param.Pointer.WriteByte(dataOffset + diffMulColorG, chrType.diffMulColor.G);
            row.Param.Pointer.WriteByte(dataOffset + diffMulColorB, chrType.diffMulColor.B);

            row.Param.Pointer.WriteByte(dataOffset + frontColorR, chrType.frontColor.R);
            row.Param.Pointer.WriteByte(dataOffset + frontColorG, chrType.frontColor.G);
            row.Param.Pointer.WriteByte(dataOffset + frontColorB, chrType.frontColor.B);

            CommandManager.Log($"Reset Color for {chrType.Name}");
        }
    }
}
