using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using Erd_Tools;
using Erd_Tools.Models;
using PropertyHook;
using PvPHelper.Core;
using PvPHelper.MVVM.Dialogs;
using static Erd_Tools.Models.Param;

namespace PvPHelper.Console.Commands
{
    public class NetPlayerCommand : CommandBase
    {
        private ErdHook hook;

        private PHPointer Session;
        private NetPlayer LocalNetPlayer;
        private NetPlayer NetPlayer1;
        private NetPlayer NetPlayer2;
        private NetPlayer NetPlayer3;
        private NetPlayer NetPlayer4;
        private NetPlayer NetPlayer5;

        private ItemCategory ArmorCat;
        private ItemCategory TalismansCat;

        private List<NetPlayer> NetPlayerList = new();
        public NetPlayerCommand(ErdHook hook)
        {
            Name = "NetPlayer Command";
            Description = "Get data about NetPlayers";
            CommandString = "/netplayer";
            RequireParams = true;
            HasParams = true;
            RequiresParamsString = new string[] { "player" };

            this.hook = hook;

            Session = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8 });
            LocalNetPlayer = new(hook, Session, 0x0*10);
            NetPlayer1 = new(hook, Session, 0x10);
            NetPlayer2 = new(hook, Session, 0x20);
            NetPlayer3 = new(hook, Session, 0x30);
            NetPlayer4 = new(hook, Session, 0x40);
            NetPlayer5 = new(hook, Session, 0x50);

            NetPlayerList.Add(LocalNetPlayer);
            NetPlayerList.Add(NetPlayer1);
            NetPlayerList.Add(NetPlayer2);
            NetPlayerList.Add(NetPlayer3);
            NetPlayerList.Add(NetPlayer4);
            NetPlayerList.Add(NetPlayer5);

            ArmorCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");
            TalismansCat = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");
        }

        protected override void OnTriggerCommandWithParameters(List<string> parameters)
        {
            if (!hook.Hooked || !hook.Loaded)
                throw new InvalidCommandException("Not hooked or loaded.");

            if (parameters[0].ToLower() == "list" && parameters.Count == 1)
            {
                CommandManager.Log("All NetPlayers in Current Session");
                foreach(NetPlayer netPlayer in NetPlayerList)
                {
                    if (netPlayer == LocalNetPlayer)
                        continue;

                    string name = netPlayer.Name;
                    int level = netPlayer.Level;

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
                foreach (NetPlayer netPlayer in NetPlayerList)
                {
                    string name = netPlayer.Name;

                    if (string.IsNullOrEmpty(name))
                        continue;

                    string commandName = GetCommandName(name);

                    if (parameters[0].ToLower() == commandName)
                    {
                        /*if (netPlayer == LocalNetPlayer)
                            continue;*/

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
                                        netPlayer.PhantomID = phantomId;
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

                                    netPlayer.PhantomID = phantomId;
                                    CommandManager.Log($"Set {name}'s phantom color.");
                                    break;
                                }
                            case "hp":
                                {
                                    CommandManager.Log($"{name}'s Current Health is {netPlayer.Health}");
                                    break;
                                }
                            case "level":
                                {
                                    CommandManager.Log($"{name}'s Current Level is {netPlayer.Level}");
                                    break;
                                }
                            case "stats":
                                {
                                    CommandManager.Log($"{name}'s Current Stats");
                                    CommandManager.Log($"Vigor: {netPlayer.Vigor}");
                                    CommandManager.Log($"Mind: {netPlayer.Mind}");
                                    CommandManager.Log($"Endurance: {netPlayer.Endurance}");
                                    CommandManager.Log($"Strength: {netPlayer.Strength}");
                                    CommandManager.Log($"Dexterity: {netPlayer.Dexterity}");
                                    CommandManager.Log($"Intelligence: {netPlayer.Intelligence}");
                                    CommandManager.Log($"Faith: {netPlayer.Faith}");
                                    CommandManager.Log($"Arcane: {netPlayer.Arcane}");
                                    break;
                                }
                            case "helmet":
                                {
                                    Item helmet = ArmorCat.Items.FirstOrDefault(x => x.ID == netPlayer.HelmetID);
                                    CommandManager.Log($"{name}'s Current Helmet is {helmet.Name}");
                                    break;
                                }
                            case "armor":
                                {
                                    Item armor = ArmorCat.Items.FirstOrDefault(x => x.ID == netPlayer.ArmorID);
                                    CommandManager.Log($"{name}'s Current Armor is {armor.Name}");
                                    break;
                                }
                            case "guantlet":
                                {
                                    Item guantlet = ArmorCat.Items.FirstOrDefault(x => x.ID == netPlayer.GauntletID);
                                    CommandManager.Log($"{name}'s Current Guantlets is {guantlet.Name}");
                                    break;
                                }
                            case "leggings":
                                {
                                    Item leggings = ArmorCat.Items.FirstOrDefault(x => x.ID == netPlayer.LeggingsID);
                                    CommandManager.Log($"{name}'s Current Leggings is {leggings.Name}");
                                    break;
                                }
                            case "tal1":
                                {
                                    Item tal = TalismansCat.Items.FirstOrDefault(x => x.ID == netPlayer.Accessory1ID);
                                    CommandManager.Log($"{name}'s First Slot Talisman is {tal.Name}");
                                    break;
                                }
                            case "tal2":
                                {
                                    Item tal = TalismansCat.Items.FirstOrDefault(x => x.ID == netPlayer.Accessory2ID);
                                    CommandManager.Log($"{name}'s Second Slot Talisman is {tal.Name}");
                                    break;
                                }
                            case "tal3":
                                {
                                    Item tal = TalismansCat.Items.FirstOrDefault(x => x.ID == netPlayer.Accessory3ID);
                                    CommandManager.Log($"{name}'s Third Slot Talisman is {tal.Name}");
                                    break;
                                }
                            case "tal4":
                                {
                                    Item tal = TalismansCat.Items.FirstOrDefault(x => x.ID == netPlayer.Accessory4ID);
                                    CommandManager.Log($"{name}'s Fourth Slot Talisman is {tal.Name}");
                                    break;
                                }
                            case "tp":
                                {
                                    LocalNetPlayer.TeleportToPlayer(netPlayer);
                                    break;
                                }
                            case "gcoords":
                                {
                                    CommandManager.Log($"{name}'s Coords");
                                    CommandManager.Log($"X: {netPlayer.GlobalX}");
                                    CommandManager.Log($"Y: {netPlayer.GlobalY}");
                                    CommandManager.Log($"Z: {netPlayer.GlobalZ}");
                                    break;
                                }
                            case "lcoords":
                                {
                                    CommandManager.Log($"{name}'s Local Coords");
                                    CommandManager.Log($"X: {netPlayer.LocalX}");
                                    CommandManager.Log($"Y: {netPlayer.LocalY}");
                                    CommandManager.Log($"Z: {netPlayer.LocalZ}");
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
