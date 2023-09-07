using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using Erd_Tools;
using Erd_Tools.Models;
using PropertyHook;
using PvPHelper.MVVM.Dialogs;
using static Erd_Tools.Models.Param;

namespace PvPHelper.Console.Commands
{
    public class NetPlayerCommand : CommandBase
    {
        private ErdHook hook;

        private PHPointer LocalNetPlayer;
        private PHPointer NetPlayer1;
        private PHPointer NetPlayer2;
        private PHPointer NetPlayer3;
        private PHPointer NetPlayer4;
        private PHPointer NetPlayer5;

        private List<PHPointer> NetPlayerList = new();
        public NetPlayerCommand(ErdHook hook)
        {
            Name = "NetPlayer Command";
            Description = "Get data about NetPlayers";
            CommandString = "/netplayer";
            RequireParams = true;
            HasParams = true;
            RequiresParamsString = new string[] { "player" };

            this.hook = hook;

            LocalNetPlayer = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8, 0x0*10 });
            NetPlayer1 = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8, 0x10 });
            NetPlayer2 = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8, 0x20 });
            NetPlayer3 = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8, 0x30 });
            NetPlayer4 = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8, 0x40 });
            NetPlayer5 = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8, 0x50 });

            NetPlayerList.Add(LocalNetPlayer);
            NetPlayerList.Add(NetPlayer1);
            NetPlayerList.Add(NetPlayer2);
            NetPlayerList.Add(NetPlayer3);
            NetPlayerList.Add(NetPlayer4);
            NetPlayerList.Add(NetPlayer5);
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Hooked || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded.");

            if (parameters[0].ToLower() == "list" && parameters.Count == 1)
            {
                CommandManager.Log("All NetPlayers in Current Session");
                foreach(PHPointer netPlayer in NetPlayerList)
                {
                    if (netPlayer == LocalNetPlayer)
                        continue;

                    PHPointer netPlayerChrData = hook.CreateChildPointer(netPlayer, new int[] { 0x580 });

                    string name = netPlayerChrData.ReadString(0x9C, Encoding.Unicode, 0x32);
                    int level = netPlayerChrData.ReadInt32(0x68);

                    if (!string.IsNullOrEmpty(name))
                    {
                        CommandManager.Log($"Name: {name}, Level: {level}");
                    }
                }
                return;
            }

            if (string.IsNullOrEmpty(parameters[0]))
                throw new InvalidCommandException("Null parameter.");

            if (parameters.Count > 1)
            {
                foreach (PHPointer netPlayer in NetPlayerList)
                {
                    PHPointer netPlayerChrData = hook.CreateChildPointer(netPlayer, new int[] { 0x580 });
                    string name = netPlayerChrData.ReadString(0x9C, Encoding.Unicode, 0x32);

                    if (string.IsNullOrEmpty(name))
                        continue;

                    string commandName = GetCommandName(name);

                    if (parameters[0].ToLower() == commandName)
                    {
                        if (netPlayer == LocalNetPlayer)
                            continue;

                        switch (parameters[1].ToLower())
                        {
                            case "setphantomcolor":
                                {
                                    if (parameters.Count != 3)
                                        throw new InvalidCommandException("Invalid parameter count");

                                    if (!int.TryParse(parameters[2], out int phantomId))
                                        throw new InvalidCommandException("Invalid Value.");

                                    ColorPickerDialog dialog = new();

                                    dialog.OnSubmit += (color)=> 
                                    { 
                                        SetPhantomColorSubmit(color, phantomId);
                                        netPlayer.WriteInt32(0x538, phantomId);
                                        CommandManager.Log($"Set {name}'s phantom color.");
                                    };
                                    dialog.ShowDialog();
                                    break;
                                }
                            case "setphantomid":
                                {
                                    if (parameters.Count != 3)
                                        throw new InvalidCommandException("Invalid parameter count");

                                    if (!int.TryParse(parameters[2], out int phantomId))
                                        throw new InvalidCommandException("Invalid Value.");

                                    netPlayer.WriteInt32(0x538, phantomId);
                                    CommandManager.Log($"Set {name}'s phantom color.");
                                    break;
                                }
                        }
                    }
                }
            }
        }

        public Param PhantomParam => hook.Params.FirstOrDefault(x => x.Name == "PhantomParam");
        public int edgeColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorR").FieldOffset;
        public int edgeColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorG").FieldOffset;
        public int edgeColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "edgeColorB").FieldOffset;
        public int diffMulColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorR").FieldOffset;
        public int diffMulColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorG").FieldOffset;
        public int diffMulColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "diffMulColorB").FieldOffset;
        public int frontColorR => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorR").FieldOffset;
        public int frontColorG => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorG").FieldOffset;
        public int frontColorB => PhantomParam.Fields.FirstOrDefault(x => x.InternalName == "frontColorB").FieldOffset;
        private void SetPhantomColorSubmit(System.Drawing.Color color, int id)
        {
            Row row = PhantomParam.Rows.FirstOrDefault(x => x.ID == id);
            var dataOffset = row.DataOffset;

            row.Param.Pointer.WriteByte(dataOffset + edgeColorR, color.R);
            row.Param.Pointer.WriteByte(dataOffset + edgeColorG, color.G);
            row.Param.Pointer.WriteByte(dataOffset + edgeColorB, color.B);

            row.Param.Pointer.WriteByte(dataOffset + diffMulColorR, 255);
            row.Param.Pointer.WriteByte(dataOffset + diffMulColorG, 255);
            row.Param.Pointer.WriteByte(dataOffset + diffMulColorB, 255);

            Color invertedColor = invertColor(color);
            row.Param.Pointer.WriteByte(dataOffset + frontColorR, (byte)Math.Round(invertedColor.R * 0.3, 0));
            row.Param.Pointer.WriteByte(dataOffset + frontColorG, (byte)Math.Round(invertedColor.G * 0.3, 0));
            row.Param.Pointer.WriteByte(dataOffset + frontColorB, (byte)Math.Round(invertedColor.B * 0.3, 0));
        }
        private Color invertColor(Color colorToInvert)
        {
            return Color.FromArgb(colorToInvert.ToArgb() ^ 0xffffff);
        }
        private string GetCommandName(string ogName)
        {
            return ogName.Replace(" ", "_").ToLower();
        }
    }
}
