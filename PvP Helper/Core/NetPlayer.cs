using Erd_Tools;
using Erd_Tools.Utils;
using PropertyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PvPHelper.Core
{
    public class NetPlayer
    {
        public PHPointer NetPlayerData { get; set; }
        public PHPointer ChrData { get; set; }
        public PHPointer SteamData { get; set; }
        private PHPointer KickOutFunction { get; set; }
        private PHPointer LocalPositionData { get; set; }

        private ErdHook Hook { get; set; }
        #region MainData
        public string Name => ChrData.ReadString(0x9C, Encoding.Unicode, 32);
        public int Level => ChrData.ReadInt32(0x68);
        public int Health => ChrData.ReadInt32(0x10);
        public int HPMax => ChrData.ReadInt32(0x14);
        #endregion
        #region Stats
        public int Vigor => ChrData.ReadInt32(0x3C);
        public int Mind => ChrData.ReadInt32(0x40);
        public int Endurance => ChrData.ReadInt32(0x44);
        public int Strength => ChrData.ReadInt32(0x48);
        public int Dexterity => ChrData.ReadInt32(0x4C);
        public int Intelligence => ChrData.ReadInt32(0x50);
        public int Faith => ChrData.ReadInt32(0x54);
        public int Arcane => ChrData.ReadInt32(0x58);
        #endregion
        #region Equipment
        public int HelmetID => ChrData.ReadInt32(0x3CC);
        public int ArmorID => ChrData.ReadInt32(0x3D0);
        public int GauntletID => ChrData.ReadInt32(0x3D4);
        public int LeggingsID => ChrData.ReadInt32(0x3D8);

        public int Accessory1ID => ChrData.ReadInt32(0x3E0);
        public int Accessory2ID => ChrData.ReadInt32(0x3E4);
        public int Accessory3ID => ChrData.ReadInt32(0x3E8);
        public int Accessory4ID => ChrData.ReadInt32(0x3EC);

        public int RWeapon1 => ChrData.ReadInt32(0x3A0);
        public int RWeapon2 => ChrData.ReadInt32(0x3A8);
        public int RWeapon3 => ChrData.ReadInt32(0x3B0);

        public int LWeapon1 => ChrData.ReadInt32(0x39C);
        public int LWeapon2 => ChrData.ReadInt32(0x3A4);
        public int LWeapon3 => ChrData.ReadInt32(0x3AC);
        #endregion
        #region Debug
        public int PhantomID
        {
            get { return NetPlayerData.ReadInt32(0x538); }
            set { NetPlayerData.WriteInt32(0x538, value); }
        }
        public float LocalX
        {
            get { return LocalPositionData.ReadSingle(0x70); }
            set { LocalPositionData.WriteSingle(0x70, value); }
        }
        public float LocalY
        {
            get { return LocalPositionData.ReadSingle(0x74); }
            set { LocalPositionData.WriteSingle(0x74, value); }
        }
        public float LocalZ
        {
            get { return LocalPositionData.ReadSingle(0x78); }
            set { LocalPositionData.WriteSingle(0x78, value); }
        }
        public float GlobalX => NetPlayerData.ReadSingle(0x6C0);
        public float GlobalY => NetPlayerData.ReadSingle(0x6C4);
        public float GlobalZ => NetPlayerData.ReadSingle(0x6C8);

        public long SteamID => SteamData.ReadInt64(0x10);
        public PHPointer SteamIDPtr { get; set; }

        public PHPointer AnimationData { get; set; }
        #endregion

        public NetPlayer(ErdHook hook, PHPointer session, int offset, PHPointer KickOutFunction = null)
        {
            Hook = hook;
            NetPlayerData = hook.CreateChildPointer(session, offset);
            ChrData = hook.CreateChildPointer(NetPlayerData, 0x580);
            AnimationData = hook.CreateChildPointer(NetPlayerData, 0x190, 0x58);
            LocalPositionData = hook.CreateChildPointer(NetPlayerData, new int[] { 0x190, 0x68 });
            SteamData = hook.CreateChildPointer(NetPlayerData, 0x6B8);
            SteamIDPtr = hook.CreateChildPointer(SteamData, 0x10);

            if (KickOutFunction != null)
                this.KickOutFunction = KickOutFunction;
        }

        public void TeleportToPlayer(NetPlayer player)
        {
            TeleportTo(player.LocalX, player.LocalY, player.LocalZ);
        }
        public void TeleportTo(float x, float y, float z)
        {
            LocalX = GlobalX + x - GlobalX;
            LocalY = GlobalY + y - GlobalY;
            LocalZ = GlobalZ + z - GlobalZ;
        }
        public void Kick()
        {
            if (KickOutFunction == null)
                return;

            string asmStr = Helpers.GetEmbededResource("Resources.Assembly.KickOutFunction.asm");
            string asm = string.Format(asmStr, CustomPointers.GlobalSessionMan.Resolve(), (SteamData.Resolve().ToInt64() + 0x10), KickOutFunction.Resolve());
            Hook.AsmExecute(asm);
        }

        public void KillTest()
        {
            AnimationData.WriteInt32(0x18, 17140);
        }
    }
}
