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
    internal class ChangeChrTypeColor : CommandBase
    {
        private ErdHook _hook;
        private DashboardViewModel _dashboard;
        public ChrType CurrChrType;

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
        public ChangeChrTypeColor(ErdHook hook, DashboardViewModel viewModel)
        {
            _hook = hook;
            _dashboard = viewModel;
        }
        public override void Execute(object? parameter)
        {
            if (!_hook.Loaded || !_hook.Hooked)
                return;

            ColorPickerDialog dialog = new();
            PhantomIDOption phantom = _dashboard.PhantomIDItemsSource.ToArray()[_dashboard.PhantomIDSelectedIndex] as PhantomIDOption;
            dialog.OnSubmit += (color) =>
            {
                if (phantom == null || phantom.ID == -1)
                    return;

                Row row = PhantomParam.Rows.FirstOrDefault(x => x.ID == phantom.ID);
                var dataOffset = row.DataOffset;

                row.Param.Pointer.WriteByte((int)dataOffset + edgeColorR, color.R);
                row.Param.Pointer.WriteByte((int)dataOffset + edgeColorG, color.G);
                row.Param.Pointer.WriteByte((int)dataOffset + edgeColorB, color.B);

                row.Param.Pointer.WriteByte((int)dataOffset + diffMulColorR, 255);
                row.Param.Pointer.WriteByte((int)dataOffset + diffMulColorG, 255);
                row.Param.Pointer.WriteByte((int)dataOffset + diffMulColorB, 255);

                Color invertedColor = invertColor(color);
                row.Param.Pointer.WriteByte((int)dataOffset + frontColorR, (byte)Math.Round(invertedColor.R * 0.3, 0));
                row.Param.Pointer.WriteByte((int)dataOffset + frontColorG, (byte)Math.Round(invertedColor.G * 0.3, 0));
                row.Param.Pointer.WriteByte((int)dataOffset + frontColorB, (byte)Math.Round(invertedColor.B * 0.3, 0));

                CommandManager.Log("Color Updated");
                CommandManager.Log($"R: {color.R} G: {color.G} B: {color.B}");
            };
            dialog.ShowDialog();
        }
        private Color invertColor(Color colorToInvert)
        {
            return Color.FromArgb(colorToInvert.ToArgb() ^ 0xffffff);
        }
    }
}
