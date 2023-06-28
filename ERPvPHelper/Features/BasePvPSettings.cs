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
using System.IO;
using System.Reflection;

namespace ERPvPHelper
{
    public class BasePvPSettings
    {
        private Form form { get; set; }
        public ErdHook hook { get; set; }
        private Logger? logger { get; set; }
        public Player? player { get; private set; }
        public PHPointer? ChrFlags { get; private set; }
        public PHPointer? idleAnimation { get; private set; }
        public PHPointer? animPointer { get; private set; }

        #region From Controls
        public CheckBox? NoDeadToggle { get; set; }
        public CheckBox? NoDamageToggle { get; set; }
        public CheckBox? NoFPConsumeToggle { get; set; }
        public CheckBox? NoStamLossToggle { get; set; }
        public CheckBox? AutoReviveToggle { get; set; }
        public CheckBox? MadHealToggle { get; set; }
        public CheckBox? NoGoodsConsumeToggle { get; set; }
        public CheckBox? SeamlessAnimChangeToggle { get; set; }
        public TextBox? ManaBox { get; set; }
        public TextBox? HealthBox { get; set; }
        #endregion


        public BasePvPSettings(Form form, ErdHook hook, Logger logger)
        {
            this.form = form;
            this.hook = hook;
            this.logger = logger;

            hook.OnHooked += OnHooked;
            hook.OnUnhooked += OnUnhooked;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ERPvPHelper.Resources.ConsumableIDS.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using(StreamReader reader = new StreamReader(stream))
            {
                consumableIds = EnumerateLines(reader).Select(x => Convert.ToInt32(x)).ToList();
            }
        }
        public static IEnumerable<string> EnumerateLines(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
        private void OnUnhooked(object? sender, PHEventArgs e)
        {
            player = null;
            form.Invoke(new Action(() =>
            {
                NoDeadToggle.Checked = false;
                NoDamageToggle.Checked = false;
                AutoReviveToggle.Checked = false;
                NoGoodsConsumeToggle.Checked = false;
                SeamlessAnimChangeToggle.Checked = false;
                NoFPConsumeToggle.Checked = false;
                NoStamLossToggle.Checked = false;
                MadHealToggle.Checked = false;
            }));
        }

        private void OnHooked(object? sender, PHEventArgs e)
        {
            logger.Log("Attached to Elden Ring");
            player = new Player(hook.PlayerIns, hook);
            logger.Log("Created new Player");

            logger.Log("Creating Pointers...");
            ChrFlags = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x0 });
            idleAnimation = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x58 });
            animPointer = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x80});
            logger.Log("Pointers Created");

            logger.Log("Starting Loops...");
            Task.Run(new Action(() => { checks(); }));
            Task.Run(new Action(() => { UpdateMana(); }));
            Task.Run(new Action(() => { UpdateHealth(); }));
            logger.Log("Done! You are free to use the tool!");
        }
        #region checks
        private void checks()
        {
            int chrFlagsOffset = 0x19B;
            int noDeadBitIndex = 0;
            int noDamageBitIndex = 1;
            int noFpConsumeBitIndex = 2;
            int noStaminaConsumeBitIndex = 3;

            while (hook.Loaded)
            {
                form.Invoke(new Action(() =>
                {
                    byte chrFlagsByte = ChrFlags.ReadByte(chrFlagsOffset);

                    if (NoDeadToggle.Checked != Helpers.IsBitSet(chrFlagsByte, noDeadBitIndex))
                    {
                        ChrFlags.WriteByte(chrFlagsOffset, Helpers.SetBit(chrFlagsByte, noDeadBitIndex, NoDeadToggle.Checked));
                    }

                    if (NoDamageToggle.Checked != Helpers.IsBitSet(chrFlagsByte, noDamageBitIndex))
                    {
                        ChrFlags.WriteByte(chrFlagsOffset, Helpers.SetBit(chrFlagsByte, noDamageBitIndex, NoDamageToggle.Checked));
                    }

                    if (NoFPConsumeToggle.Checked != Helpers.IsBitSet(chrFlagsByte, noFpConsumeBitIndex))
                    {
                        ChrFlags.WriteByte(chrFlagsOffset, Helpers.SetBit(chrFlagsByte, noFpConsumeBitIndex, NoFPConsumeToggle.Checked));
                    }

                    if (NoStamLossToggle.Checked != Helpers.IsBitSet(chrFlagsByte, noStaminaConsumeBitIndex))
                    {
                        ChrFlags.WriteByte(chrFlagsOffset, Helpers.SetBit(chrFlagsByte, noStaminaConsumeBitIndex, NoStamLossToggle.Checked));
                    }
                }));
                Thread.Sleep(10000);
            }
            return;
        }
        #endregion
        #region Mana/Health ManualSet
        public void SetMana(string newMana)
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                return;
            }

            int newSetMana;
            if (!int.TryParse(newMana, out newSetMana))
            {
                MessageBox.Show("Couldnt Parse newSetMana, try something else");
                logger.Log("Couldnt Parse newSetMana, often means you put a invalid character.");
                return;
            }

            int maxFp = player.FpMax;

            if (newSetMana > maxFp || newSetMana < 0)
            {
                MessageBox.Show("Invalid FP Amount, try something else");
                logger.Log("Invalid FP Amount, often means you tried to put more than your max or less than 0");
                return;
            }

            player.Fp = newSetMana;
        }
        public void SetHealth(string newHealth)
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                return;
            }

            int newSetHealth;
            if (!int.TryParse(newHealth, out newSetHealth))
            {
                MessageBox.Show("Couldnt Parse newSetHealth, try something else");
                logger.Log("Couldnt Parse newSetHealth, often means you put an invalid character");
                return;
            }

            int maxHP = player.HpMax;

            if (newSetHealth > maxHP || newSetHealth < 0)
            {
                MessageBox.Show("Invalid HP Amount, try something else");
                logger.Log("Invalid HP Amount, often means you tried putting more than your max or less than 0.");
                return;
            }

            player.Hp = newSetHealth;
        }
        #endregion
        #region Updates
        private void UpdateMana()
        {
            while (hook.Hooked)
            {
                if (hook.Loaded)
                    form.Invoke(new Action(() => { ManaBox.Text = player.Fp.ToString(); }));

                Thread.Sleep(3000);
            }
        }
        private void UpdateHealth()
        {
            while (hook.Hooked)
            {
                if (hook.Loaded)
                    form.Invoke(new Action(() => { HealthBox.Text = player.Hp.ToString(); }));

                Thread.Sleep(3000);
            }
        }
        private void AutoReviveTimer(Player tempPlayer)
        {
            while (AutoReviveToggle.Checked)
            {
                if (!hook.Hooked)
                    return;
                if (hook.Loaded)
                {
                    int anim = animPointer.ReadInt32(0x90);
                    if (anim == 70000 || anim == 70010)
                    {
                        form.Invoke(new Action(() =>
                        {
                            byte b = ChrFlags.ReadByte(0x19B);

                            ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, true));
                        }));
                        if (tempPlayer.Hp <= 1)
                        {
                            Thread.Sleep(1000);
                            Revive(tempPlayer);
                        }
                    }
                    else
                    {
                        byte b = ChrFlags.ReadByte(0x19B);

                        ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, false));
                    }

                    if (tempPlayer.Hp == 0)
                    {
                        Thread.Sleep(1000);
                        Revive(tempPlayer);
                        logger.Log("Revived Player.");
                    }
                }
            }

            return;
        }
        #endregion
        #region commands
        public void HealToMax()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                return;
            }
            int hpBefore = player.Hp;
            player.Hp = player.HpMax;

            logger.Log($"Healed Player for {player.HpMax - hpBefore} HP.");
        }

        public void RefillFPToMax()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                return;
            }

            int fpBefore = player.Fp;
            player.Fp = player.FpMax;

            logger.Log($"Refilled Mana for {player.FpMax - fpBefore} FP.");
        }
        public void Revive(Player currPlayer)
        {
            currPlayer.Hp = currPlayer.HpMax;
            currPlayer.Fp = currPlayer.FpMax;
            idleAnimation.WriteInt32(0x18, 0);
        }
        #endregion
        #region Toggles
        public void NoDeadtoggle()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                NoDeadToggle.CheckState = CheckState.Unchecked;
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                NoDeadToggle.CheckState = CheckState.Unchecked;
                return;
            }
            byte b = ChrFlags.ReadByte(0x19B);

            ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 0, NoDeadToggle.Checked));

            logger.Log($"No Dead Toggled {NoDeadToggle.Checked}.");
        }
        public void InfiniteFPToggle()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                NoFPConsumeToggle.CheckState = CheckState.Unchecked;
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                NoFPConsumeToggle.CheckState = CheckState.Unchecked;
                return;
            }
            byte b = ChrFlags.ReadByte(0x19B);

            ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 2, NoFPConsumeToggle.Checked));
            logger.Log($"No FP Loss Toggle {NoFPConsumeToggle.Checked}");
        }
        public void NoDamagetoggle()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                NoDamageToggle.CheckState = CheckState.Unchecked;
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                NoDamageToggle.CheckState = CheckState.Unchecked;
                return;
            }
            byte b = ChrFlags.ReadByte(0x19B);
            ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 1, NoDamageToggle.Checked));

            logger.Log($"No Damage Toggled {NoDamageToggle.Checked}");
        }
        public void InfiniteStamToggle()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                NoStamLossToggle.CheckState = CheckState.Unchecked;
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                NoStamLossToggle.CheckState = CheckState.Unchecked;
                return;
            }
            byte b = ChrFlags.ReadByte(0x19B);
            ChrFlags.WriteByte(0x19B, Helpers.SetBit(b, 3, NoStamLossToggle.Checked));

            logger.Log($"No Stam Loss Toggled {NoStamLossToggle.Checked}");
        }
        private List<Row> savedRows = new List<Row>();
        public List<int> consumableIds = new();
        public void InfiniteConsumablesToggle()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                NoGoodsConsumeToggle.CheckState = CheckState.Unchecked;
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                NoGoodsConsumeToggle.CheckState = CheckState.Unchecked;
                return;
            }

            logger.Log($"Changing consumbables 'isConsume' param to {(NoGoodsConsumeToggle.Checked ? "0" : "1")}..");

            /*byte noConsumeChrParam = hook.PlayerIns.ReadByte(0x522);
            hook.PlayerIns.WriteByte(0x522, Helpers.SetBit(noConsumeChrParam, 0, NoGoodsConsumeToggle.Checked));*/

            var bitIndex = 7;
            var consumeOffset = hook.EquipParamGoods.Fields[40].FieldOffset;

            for (int i = 0; i < consumableIds.Count - 1; i++)
            {
                var row = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.ID == consumableIds[i]);
                var dataOffset = row.DataOffset;

                byte b = row.Param.Pointer.ReadByte(dataOffset + consumeOffset);
                b = Helpers.SetBit(b, bitIndex, NoGoodsConsumeToggle.Checked ? false : true);

                row.Param.Pointer.WriteByte(dataOffset + consumeOffset, b);
                //Log($"Made isConsume {(NoGoodsConsumeToggle.Checked ? "false" : "true")} for {row.Name}");
            }

            logger.Log($"Infinite Consumables Toggled {NoGoodsConsumeToggle.Checked}.");
        }
        public void autoReviveToggle()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                AutoReviveToggle.CheckState = CheckState.Unchecked;
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                AutoReviveToggle.CheckState = CheckState.Unchecked;
                return;
            }

            if (AutoReviveToggle.Checked)
                Task.Run(new Action(() => AutoReviveTimer(player)));

            logger.Log($"Auto Revive Toggled {AutoReviveToggle.Checked}.");
        }

        public void SeamlessAnimToggle()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                SeamlessAnimChangeToggle.CheckState = CheckState.Unchecked;
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                SeamlessAnimChangeToggle.CheckState = CheckState.Unchecked;
                return;
            }

            var seamlessRuleBook = hook.EquipParamGoods.Rows[8];
            var consumeOffset = seamlessRuleBook.Param.Fields[28].FieldOffset;
            var dataOffset = seamlessRuleBook.DataOffset;

            if (SeamlessAnimChangeToggle.Checked)
            {
                seamlessRuleBook.Param.Pointer.WriteByte(dataOffset + consumeOffset, 16);
            }
            else
            {
                seamlessRuleBook.Param.Pointer.WriteByte(dataOffset + consumeOffset, 60);
            }

            logger.Log($"Judicator's Rulebook animation changed to {(SeamlessAnimChangeToggle.Checked ? "'Quick'" : "'Prayer'")} Animation.");
        }
        #region MadHeal
        private Dictionary<string, int> softCottonDefaultParams = new();
        public void madHealToggle()
        {
            if (!hook.Hooked)
            {
                logger.Log("You have not attached to Elden Ring. Please attach first.", Logger.LogType.Error);
                MadHealToggle.CheckState = CheckState.Unchecked;
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Your character has not been loaded. Please load a save.", Logger.LogType.Error);
                MadHealToggle.CheckState = CheckState.Unchecked;
                return;
            }

            int[] larIds = new int[] { 10673000, 10673001, 10673002, 10673010, 10673011, 10673012, 10673013 };
            Param bulletParam = hook.Params.FirstOrDefault(x => x.Name == "Bullet");
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

            var softCotton = hook.EquipParamGoods.Rows.FirstOrDefault(x => x.Name == "2100 - Soft Cotton");
            var dataOffset = softCotton.DataOffset;

            var refId1Offset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("refId_default"));
            var iconIdOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("iconId"));
            var goodsUseAnimOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("goodsUseAnim"));
            var consumeMPOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("consumeMP"));
            var refCategoryOffset = hook.EquipParamGoods.Fields.FirstOrDefault(x => x.InternalName.Contains("refCategory"));

            if (MadHealToggle.Checked && softCottonDefaultParams.Count == 0)
            {
                softCottonDefaultParams.Add("refId1", softCotton.Param.Pointer.ReadInt32(dataOffset + refId1Offset.FieldOffset));
                softCottonDefaultParams.Add("iconId", softCotton.Param.Pointer.ReadInt16(dataOffset + iconIdOffset.FieldOffset));
                softCottonDefaultParams.Add("goodsUseAnim", softCotton.Param.Pointer.ReadByte(dataOffset + goodsUseAnimOffset.FieldOffset));
                softCottonDefaultParams.Add("consumeMp", softCotton.Param.Pointer.ReadInt16(dataOffset + consumeMPOffset.FieldOffset));
                softCottonDefaultParams.Add("refCategory", softCotton.Param.Pointer.ReadByte(dataOffset + refCategoryOffset.FieldOffset));
            }

            softCotton.Param.Pointer.WriteInt32(dataOffset + refId1Offset.FieldOffset, MadHealToggle.Checked ? larIds[0] : softCottonDefaultParams["refId1"]);
            softCotton.Param.Pointer.WriteInt16(dataOffset + iconIdOffset.FieldOffset, (short)(MadHealToggle.Checked ? 6064 : softCottonDefaultParams["iconId"]));
            softCotton.Param.Pointer.WriteByte(dataOffset + goodsUseAnimOffset.FieldOffset, (byte)(MadHealToggle.Checked ? 16 : softCottonDefaultParams["goodsUseAnim"]));
            softCotton.Param.Pointer.WriteInt16(dataOffset + consumeMPOffset.FieldOffset, (short)(MadHealToggle.Checked ? 0 : softCottonDefaultParams["consumeMp"]));
            softCotton.Param.Pointer.WriteByte(dataOffset + refCategoryOffset.FieldOffset, (byte)(MadHealToggle.Checked ? 1 : softCottonDefaultParams["refCategory"]));

            if (MadHealToggle.Checked)
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


            lawOfReg1.Param.Pointer.WriteInt32(lawOfReg1.DataOffset + spEffectIdforShooterOffset.FieldOffset, MadHealToggle.Checked ? 110 : 0);
            lawOfReg1.Param.Pointer.WriteInt32(lawOfReg1.DataOffset + hitBulletIdOffset.FieldOffset, MadHealToggle.Checked ? larIds[1] : larIds[0]);

            lawOfReg2.Param.Pointer.WriteInt32(lawOfReg2.DataOffset + hitBulletIdOffset.FieldOffset, MadHealToggle.Checked ? larIds[2] : larIds[1]);

            lawOfReg3.Param.Pointer.WriteInt32(lawOfReg3.DataOffset + hitBulletIdOffset.FieldOffset, MadHealToggle.Checked ? larIds[3] : larIds[2]);

            lawOfReg4.Param.Pointer.WriteInt32(lawOfReg4.DataOffset + hitBulletIdOffset.FieldOffset, MadHealToggle.Checked ? larIds[4] : larIds[3]);

            lawOfReg5.Param.Pointer.WriteInt32(lawOfReg5.DataOffset + hitBulletIdOffset.FieldOffset, MadHealToggle.Checked ? larIds[5] : larIds[4]);

            lawOfReg6.Param.Pointer.WriteInt32(lawOfReg6.DataOffset + hitBulletIdOffset.FieldOffset, MadHealToggle.Checked ? larIds[6] : larIds[5]);

            lawOfReg7.Param.Pointer.WriteInt32(lawOfReg7.DataOffset + spEffectId0Offset.FieldOffset, MadHealToggle.Checked ? 110 : 503041);
        }
        #endregion
        #endregion
    }
}
