using Erd_Tools;
using Erd_Tools.Models.Entities;
using PropertyHook;
using PvPHelper.Console;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Core.Extensions
{
    public static class PlayerExtentsions
    {
        public static int GetLevel(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Level);
        public static int GetVigor(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Vigor);
        public static int GetMind(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Mind);
        public static int GetEndurance(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Endurance);
        public static int GetStrength(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Strength);
        public static int GetDexterity(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Dexterity);
        public static int GetIntelligence(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Intelligence);
        public static int GetFaith(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Faith);
        public static int GetArcane(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Arcane);
        public static int GetRunes(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.Soul);
        public static int GetTotalRunes(this Player player) => player.ChrData.ReadInt32((int)PlayerOffsets.TotalGetSoul);

        public static void AddRunes(this Player player, int amount)
        {
            string asmString = Helpers.GetEmbededResource("Resources.Assembly.AddRunes.asm");
            string asm = string.Format(asmString, ExtensionsCore.GetMainHook().PlayerGameData.Resolve(), amount, ExtensionsCore.AddSoul_Call.Resolve());
            ExtensionsCore.GetMainHook().AsmExecute(asm);
        }

        public enum PlayerOffsets
        {
            Level = 0x68,
            Vigor = 0x3C,
            Mind = 0x40,
            Endurance = 0x44,
            Strength = 0x48,
            Dexterity = 0x4C,
            Intelligence = 0x50,
            Faith = 0x54,
            Arcane = 0x58,
            Soul = 0x6C,
            TotalGetSoul = 0x70
        }

        public static int[] GetAllSpecialEffects(this Player player)
        {
            ErdHook hook = ExtensionsCore.GetMainHook();

            if (!hook.Loaded)
                return null;

            PHPointer sfx = player._chrSpecialEffects;

            bool searching = true;
            PHPointer index = hook.CreateChildPointer(sfx, 0x8);

            List<int> ids = new();
            while (searching)
            {
                int id = index.ReadInt32(0x8);
                ids.Add(id);

                index = hook.CreateChildPointer(index, 0x30);
                if (index.ReadInt32(0x8) == 0)
                    searching = false;
            }

            return ids.ToArray();
        }
    }
}
