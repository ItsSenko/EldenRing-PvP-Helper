using Erd_Tools;
using Erd_Tools.Models;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Linq;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    internal class MadHealToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook _hook;

        public MadHealToggle(ErdHook hook)
        {
            _hook = hook;
        }

        private Dictionary<string, int> softCottonDefaultParams = new();
        public override void Execute(object? parameter)
        {
            if (!_hook.Hooked || !_hook.Loaded)
            {
                State = false;
                return;
            }

            int[] larIds = new int[] { 10673000, 10673001, 10673002, 10673010, 10673011, 10673012, 10673013 };
            Param bulletParam = _hook.Params.FirstOrDefault(x => x.Name == "Bullet");
            var sfxBulletOffset = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("sfxId_Bullet"));
            var sfxHitOffset = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("sfxId_Hit"));
            var lifeOffset = bulletParam.Fields.FirstOrDefault(x => x.InternalName == "life");
            var hitRadiusOffset = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("hitRadius"));
            var hitBulletIdOffset = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("HitBulletID"));
            var spEffectIdforShooterOffset = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("spEffectIDForShooter"));
            var spEffectId0Offset = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("spEffectId0"));

            var lawOfReg1 = bulletParam.Rows.FirstOrDefault(x => x.ID == larIds[0]);
            var lawOfReg2 = bulletParam.Rows.FirstOrDefault(x => x.ID == larIds[1]);
            var lawOfReg3 = bulletParam.Rows.FirstOrDefault(x => x.ID == larIds[2]);
            var lawOfReg4 = bulletParam.Rows.FirstOrDefault(x => x.ID == larIds[3]);
            var lawOfReg5 = bulletParam.Rows.FirstOrDefault(x => x.ID == larIds[4]);
            var lawOfReg6 = bulletParam.Rows.FirstOrDefault(x => x.ID == larIds[5]);
            var lawOfReg7 = bulletParam.Rows.FirstOrDefault(x => x.ID == larIds[6]);

            var softCotton = _hook.EquipParamGoods.Rows.FirstOrDefault(x => x.Name == "2100 - Soft Cotton");
            var dataOffset = softCotton.DataOffset;

            var refId1Offset = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("refId_default"));
            var iconIdOffset = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("iconId"));
            var goodsUseAnimOffset = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("goodsUseAnim"));
            var consumeMPOffset = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("consumeMP"));
            var refCategoryOffset = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("refCategory"));

            if (State && softCottonDefaultParams.Count == 0)
            {
                softCottonDefaultParams.Add("refId1", softCotton.Param.Pointer.ReadInt32(dataOffset + refId1Offset.FieldOffset));
                softCottonDefaultParams.Add("iconId", softCotton.Param.Pointer.ReadInt16(dataOffset + iconIdOffset.FieldOffset));
                softCottonDefaultParams.Add("goodsUseAnim", softCotton.Param.Pointer.ReadByte(dataOffset + goodsUseAnimOffset.FieldOffset));
                softCottonDefaultParams.Add("consumeMp", softCotton.Param.Pointer.ReadInt16(dataOffset + consumeMPOffset.FieldOffset));
                softCottonDefaultParams.Add("refCategory", softCotton.Param.Pointer.ReadByte(dataOffset + refCategoryOffset.FieldOffset));
            }

            softCotton.Param.Pointer.WriteInt32(dataOffset + refId1Offset.FieldOffset, State ? larIds[0] : softCottonDefaultParams["refId1"]);
            softCotton.Param.Pointer.WriteInt16(dataOffset + iconIdOffset.FieldOffset, (short)(State ? 6064 : softCottonDefaultParams["iconId"]));
            softCotton.Param.Pointer.WriteByte(dataOffset + goodsUseAnimOffset.FieldOffset, (byte)(State ? 16 : softCottonDefaultParams["goodsUseAnim"]));
            softCotton.Param.Pointer.WriteInt16(dataOffset + consumeMPOffset.FieldOffset, (short)(State ? 0 : softCottonDefaultParams["consumeMp"]));
            softCotton.Param.Pointer.WriteByte(dataOffset + refCategoryOffset.FieldOffset, (byte)(State ? 1 : softCottonDefaultParams["refCategory"]));

            if (State)
            {
                foreach (var id in larIds)
                {
                    var lar = bulletParam.Rows.FirstOrDefault(x => x.ID == id);
                    lar.Param.Pointer.WriteInt32(lar.DataOffset + sfxBulletOffset.FieldOffset, 0);
                    lar.Param.Pointer.WriteInt32(lar.DataOffset + sfxHitOffset.FieldOffset, 0);
                    lar.Param.Pointer.WriteSingle(lar.DataOffset + lifeOffset.FieldOffset, .01f);
                    lar.Param.Pointer.WriteSingle(lar.DataOffset + hitRadiusOffset.FieldOffset, 30);
                }
            }
            else
            {
                lawOfReg1.Param.Pointer.WriteInt32(lawOfReg1.DataOffset + sfxBulletOffset.FieldOffset, 528161);
                lawOfReg1.Param.Pointer.WriteInt32(lawOfReg1.DataOffset + sfxHitOffset.FieldOffset, -1);
                lawOfReg1.Param.Pointer.WriteSingle(lawOfReg1.DataOffset + lifeOffset.FieldOffset, 0.1f);
                lawOfReg1.Param.Pointer.WriteSingle(lawOfReg1.DataOffset + hitRadiusOffset.FieldOffset, 1);

                lawOfReg2.Param.Pointer.WriteInt32(lawOfReg2.DataOffset + sfxBulletOffset.FieldOffset, -1);
                lawOfReg2.Param.Pointer.WriteInt32(lawOfReg2.DataOffset + sfxHitOffset.FieldOffset, -1);
                lawOfReg2.Param.Pointer.WriteSingle(lawOfReg2.DataOffset + lifeOffset.FieldOffset, 0.1f);
                lawOfReg2.Param.Pointer.WriteSingle(lawOfReg2.DataOffset + hitRadiusOffset.FieldOffset, 1);

                lawOfReg3.Param.Pointer.WriteInt32(lawOfReg3.DataOffset + sfxBulletOffset.FieldOffset, -1);
                lawOfReg3.Param.Pointer.WriteInt32(lawOfReg3.DataOffset + sfxHitOffset.FieldOffset, -1);
                lawOfReg3.Param.Pointer.WriteSingle(lawOfReg3.DataOffset + lifeOffset.FieldOffset, 0.1f);
                lawOfReg3.Param.Pointer.WriteSingle(lawOfReg3.DataOffset + hitRadiusOffset.FieldOffset, 1);

                lawOfReg4.Param.Pointer.WriteInt32(lawOfReg4.DataOffset + sfxBulletOffset.FieldOffset, -1);
                lawOfReg4.Param.Pointer.WriteInt32(lawOfReg4.DataOffset + sfxHitOffset.FieldOffset, -1);
                lawOfReg4.Param.Pointer.WriteSingle(lawOfReg4.DataOffset + lifeOffset.FieldOffset, 1.5f);
                lawOfReg4.Param.Pointer.WriteSingle(lawOfReg4.DataOffset + hitRadiusOffset.FieldOffset, 15);

                lawOfReg5.Param.Pointer.WriteInt32(lawOfReg5.DataOffset + sfxBulletOffset.FieldOffset, -1);
                lawOfReg5.Param.Pointer.WriteInt32(lawOfReg5.DataOffset + sfxHitOffset.FieldOffset, -1);
                lawOfReg5.Param.Pointer.WriteSingle(lawOfReg5.DataOffset + lifeOffset.FieldOffset, 1.5f);
                lawOfReg5.Param.Pointer.WriteSingle(lawOfReg5.DataOffset + hitRadiusOffset.FieldOffset, 15);

                lawOfReg6.Param.Pointer.WriteInt32(lawOfReg6.DataOffset + sfxBulletOffset.FieldOffset, -1);
                lawOfReg6.Param.Pointer.WriteInt32(lawOfReg6.DataOffset + sfxHitOffset.FieldOffset, -1);
                lawOfReg6.Param.Pointer.WriteSingle(lawOfReg6.DataOffset + lifeOffset.FieldOffset, 1.5f);
                lawOfReg6.Param.Pointer.WriteSingle(lawOfReg6.DataOffset + hitRadiusOffset.FieldOffset, 15);

                lawOfReg7.Param.Pointer.WriteInt32(lawOfReg7.DataOffset + sfxBulletOffset.FieldOffset, -1);
                lawOfReg7.Param.Pointer.WriteInt32(lawOfReg7.DataOffset + sfxHitOffset.FieldOffset, -1);
                lawOfReg7.Param.Pointer.WriteSingle(lawOfReg7.DataOffset + lifeOffset.FieldOffset, 1.5f);
                lawOfReg7.Param.Pointer.WriteSingle(lawOfReg7.DataOffset + hitRadiusOffset.FieldOffset, 15);
            }


            lawOfReg1.Param.Pointer.WriteInt32(lawOfReg1.DataOffset + spEffectIdforShooterOffset.FieldOffset, State ? 110 : 0);
            lawOfReg1.Param.Pointer.WriteInt32(lawOfReg1.DataOffset + hitBulletIdOffset.FieldOffset, State ? larIds[1] : larIds[0]);

            lawOfReg2.Param.Pointer.WriteInt32(lawOfReg2.DataOffset + hitBulletIdOffset.FieldOffset, State ? larIds[2] : larIds[1]);

            lawOfReg3.Param.Pointer.WriteInt32(lawOfReg3.DataOffset + hitBulletIdOffset.FieldOffset, State ? larIds[3] : larIds[2]);

            lawOfReg4.Param.Pointer.WriteInt32(lawOfReg4.DataOffset + hitBulletIdOffset.FieldOffset, State ? larIds[4] : larIds[3]);

            lawOfReg5.Param.Pointer.WriteInt32(lawOfReg5.DataOffset + hitBulletIdOffset.FieldOffset, State ? larIds[5] : larIds[4]);

            lawOfReg6.Param.Pointer.WriteInt32(lawOfReg6.DataOffset + hitBulletIdOffset.FieldOffset, State ? larIds[6] : larIds[5]);

            lawOfReg7.Param.Pointer.WriteInt32(lawOfReg7.DataOffset + spEffectId0Offset.FieldOffset, State ? 110 : 503041);
        }
    }
}
