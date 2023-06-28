using System.Collections;
using System.Diagnostics;
using Erd_Tools;
using Erd_Tools.Models;
using Erd_Tools.Models.Entities;
using Erd_Tools.Models.Items;
using PropertyHook;
using SoulsFormats;
using static Erd_Tools.Models.Param;
using System.Runtime.InteropServices;
using System.Text;

namespace ERPvPHelper
{
    public partial class OtherOptionsForm : Form
    {
        private ErdHook hook { get; set; }
        private Player player { get; set; }
        private Logger logger { get; set; }
        private Param PhantomParam { get; set; }

        int edgeColorR;
        int edgeColorG;
        int edgeColorB;

        int diffMulColorR;
        int diffMulColorG;
        int diffMulColorB;

        int frontColorR;
        int frontColorG;
        int frontColorB;

        public OtherOptionsForm(ErdHook hook, Player player, Logger logger)
        {
            InitializeComponent();

            this.hook = hook;
            this.player = player;
            this.logger = logger;

            ChrTypeSelection.DataSource = new ChrType[]
            {
                new ChrType{ChrID = 0, 
                    Name = "Normal"},

                new ChrType{ChrID = 1, 
                    Name = "Yellow Phantom", 
                    ParamID = 61, 
                    edgeColor = Color.FromArgb(200, 120, 80), 
                    diffMulColor = Color.FromArgb(255, 255, 255)},

                new ChrType{ChrID = 2, 
                    Name = "Red Phantom", 
                    ParamID = 60, 
                    edgeColor = Color.FromArgb(255, 40, 40), 
                    diffMulColor = Color.FromArgb(255, 210, 190),
                    frontColor = Color.FromArgb(0, 65, 95)},

                new ChrType{ChrID = 17, 
                    Name = "Blue Phantom", 
                    ParamID = 70, 
                    edgeColor = Color.FromArgb(112, 118, 255), 
                    diffMulColor = Color.FromArgb(255, 255, 255), 
                    frontColor = Color.FromArgb(62, 65, 0)},
            };
            PhantomParam = hook.Params.FirstOrDefault(x => x.Name == "PhantomParam");

            edgeColorR = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorR").FieldOffset;
            edgeColorG = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorG").FieldOffset;
            edgeColorB = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorB").FieldOffset;

            diffMulColorR = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorR").FieldOffset;
            diffMulColorG = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorG").FieldOffset;
            diffMulColorB = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorB").FieldOffset;

            frontColorR = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorR").FieldOffset;
            frontColorG = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorG").FieldOffset;
            frontColorB = PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorB").FieldOffset;
        }
        private void OtherOptionsForm_Load(object sender, EventArgs e)
        {
            this.TopLevel = true;
            this.TopMost = true;
        }

        private void ChrTypeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!hook.Hooked)
                return;

            ChrType type = (ChrType)ChrTypeSelection.SelectedItem;
            player.ChrType = type.ChrID;

            if (type.ChrID != 0)
                logger.Log("Please note that if you do not change back to normal before quiting. You character will save as this the next time you open the game.");
        }

        private void CustomColor_Click(object sender, EventArgs e)
        {
            ColorDialog newDialog = new ColorDialog();
            var result = newDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                
                ChrType chrType = (ChrType)ChrTypeSelection.SelectedItem;
                var phantomID = chrType.ParamID;

                Row row = PhantomParam.Rows.FirstOrDefault(x => x.ID == phantomID);
                var dataOffset = row.DataOffset;

                row.Param.Pointer.WriteByte(dataOffset + edgeColorR, newDialog.Color.R);
                row.Param.Pointer.WriteByte(dataOffset + edgeColorG, newDialog.Color.G);
                row.Param.Pointer.WriteByte(dataOffset + edgeColorB, newDialog.Color.B);

                row.Param.Pointer.WriteByte(dataOffset + diffMulColorR, 255);
                row.Param.Pointer.WriteByte(dataOffset + diffMulColorG, 255);
                row.Param.Pointer.WriteByte(dataOffset + diffMulColorB, 255);

                Color invertedColor = invertColor(newDialog.Color);
                row.Param.Pointer.WriteByte(dataOffset + frontColorR, (byte)Math.Round(invertedColor.R * 0.3, 0));
                row.Param.Pointer.WriteByte(dataOffset + frontColorG, (byte)Math.Round(invertedColor.G * 0.3, 0));
                row.Param.Pointer.WriteByte(dataOffset + frontColorB, (byte)Math.Round(invertedColor.B * 0.3, 0));
            }
        }
        private Color invertColor(Color colorToInvert)
        {
            return Color.FromArgb(colorToInvert.ToArgb() ^ 0xffffff);
        }
        private void ResetColorBtn_Click(object sender, EventArgs e)
        {
            ChrType chrType = (ChrType)ChrTypeSelection.SelectedItem;
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
        }
    }

    
}
