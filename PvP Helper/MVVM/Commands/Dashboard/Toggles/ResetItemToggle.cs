using Erd_Tools;
using Erd_Tools.Models;
using PvPHelper.Console;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Linq;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.Dashboard.Toggles
{
    internal class ResetItemToggle : CommandBase, IToggleable
    {
        private bool _state;
        public bool State { get => _state; set => SetField(ref _state, value); }
        private ErdHook _hook;

        public ResetItemToggle(ErdHook hook)
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

            int[] SpEffects = 
            {
                110, // Grace Heal
                101, // Grace Remove Active Status Effects
                1673000, //Law of Regression Visual Effect
                1673014, //Law of Regression Remove all Active Effects, buffs/debuffs etc.
                502100, //Soft Cotton 1
                502102 //Soft Cotton 2
            };

            int blessingOfMarikaIconID = 756;
            int softCottonIconID = 591;
            int softCottonBulletID = 10210000;

            Param bulletParam = _hook.Params.FirstOrDefault(x => x.Name == "Bullet");
            var spEffectIdforShooter = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("spEffectIDForShooter"));
            var spEffectId0 = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("spEffectId0"));
            var spEffectId1 = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("spEffectId1"));
            var spEffectId2 = bulletParam.Fields.FirstOrDefault(x => x.InternalName.Contains("spEffectId2"));
            
            var softCotton = _hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == 2100);
            var cottonDataOffset = softCotton.DataOffset;

            var iconId = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("iconId"));
            var goodsUseAnim = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("goodsUseAnim"));
            var isConsume = _hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName == "isConsume");

            softCotton.Param.Pointer.WriteInt16((int)cottonDataOffset + iconId.FieldOffset, (short)(State ? blessingOfMarikaIconID : softCottonIconID));
            softCotton.Param.Pointer.WriteByte((int)cottonDataOffset + goodsUseAnim.FieldOffset, (byte)(State ? 16 : 66));

            byte b = softCotton.Param.Pointer.ReadByte((int)cottonDataOffset + isConsume.FieldOffset);
            b = Helpers.SetBit(b, 7, State ? false : true);

            softCotton.Param.Pointer.WriteByte((int)cottonDataOffset + isConsume.FieldOffset, b);

            var softCottonBullet = bulletParam.Rows.FirstOrDefault(x => x.ID == softCottonBulletID);
            var cottonBulletOffset = softCottonBullet.DataOffset;

            //Special Effect Replacement
            bulletParam.Pointer.WriteInt32((int)cottonBulletOffset + spEffectIdforShooter.FieldOffset, State ? SpEffects[0] : -1);
            bulletParam.Pointer.WriteInt32((int)cottonBulletOffset + spEffectId0.FieldOffset, State ? SpEffects[1] : SpEffects[4]);
            bulletParam.Pointer.WriteInt32((int)cottonBulletOffset + spEffectId1.FieldOffset, State ? SpEffects[2] : SpEffects[5]);
            bulletParam.Pointer.WriteInt32((int)cottonBulletOffset + spEffectId2.FieldOffset, State ? SpEffects[3] : 0);

            CommandManager.Log($"Replaced {(State ? "Soft Cotten" : "Reset Item")} with {(State ? "Reset Item" : "Soft Cotten")}");
        }
    }
}
