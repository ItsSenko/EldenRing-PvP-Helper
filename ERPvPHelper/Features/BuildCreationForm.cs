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
using static Erd_Tools.Models.Weapon;
using ERPvPHelper.Features;
using System.Reflection;

namespace ERPvPHelper
{
    public partial class BuildCreationForm : Form
    {
        private ErdHook hook { get; set; }
        private Player player { get; set; }
        private Logger logger { get; set; }
        private bool IsUpdatingStats { get; set; }
        private List<int> mapEventFlagIDs = new();
        public BuildCreationForm(ErdHook hook, Player player, Logger logger)
        {
            InitializeComponent();

            this.hook = hook;
            this.player = player;
            this.logger = logger;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ERPvPHelper.Resources.MapEventFlagIds.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                mapEventFlagIDs = BasePvPSettings.EnumerateLines(reader).Select(x => Convert.ToInt32(x.Substring(0, 5))).ToList();
            }
        }

        private void BuildCreationForm_Load(object sender, EventArgs e)
        {
            Task.Run(new Action(() => StatsCheck()));
            Task.Run(new Action(() => Checks()));

            RefreshBtn.PerformClick();
        }
        private void Checks()
        {
            while(IsHandleCreated)
            {
                if (hook.Loaded && !IsUpdatingStats)
                {
                    Task.Run(new Action(() => StatsCheck()));
                }
            }
        }
        private void StatsCheck()
        {
            while(hook.Loaded && this.IsHandleCreated)
            {
                Invoke(new Action(() =>
                {
                    IsUpdatingStats = true;
                    PlayerNameLabel.Text = $"{player.PlayerName} : {player.SoulLevel}";

                    VigorLabel.Text = $"Vigor: {player.Vigor}";
                    MindLabel.Text = $"Mind: {player.Mind}";
                    EnduranceLabel.Text = $"Endurance: {player.Endurance}";
                    StrengthLabel.Text = $"Strength: {player.Strength}";
                    DexterityLabel.Text = $"Dexterity: {player.Dexterity}";
                    IntelligenceLabel.Text = $"Intelligence: {player.Intelligence}";
                    FaithLabel.Text = $"Faith: {player.Faith}";
                    ArcaneLabel.Text = $"Arcane: {player.Arcane}";

                    RunesLabel.Text = $"Runes: {player.HeldRunes}";
                }));

                Thread.Sleep(1000);
            }
            if (IsHandleCreated)
                Invoke(new Action(() => { IsUpdatingStats = false; }));
        }
        private ItemGibPage itemGibPage;
        private void ItemGibBtn_Click(object sender, EventArgs e)
        {
            if (hook.Hooked && hook.Loaded)
            {
                if (itemGibPage != null)
                {
                    itemGibPage.Close();
                    itemGibPage = null;
                }
                itemGibPage = new(hook);
                itemGibPage.TopLevel = true;

                itemGibPage.Show();
            }
        }

        private void FlaskUpgBtn_Click(object sender, EventArgs e)
        {
            if (!hook.Hooked || !hook.Loaded)
                return;
            ItemCategory upgradeMats = ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials");

            Item goldenSeed = upgradeMats.Items.FirstOrDefault(x => x.Name == "Golden Seed");
            Item sacredTear = upgradeMats.Items.FirstOrDefault(x => x.Name == "Sacred Tear");

            hook.GetItem(new(goldenSeed.ID, goldenSeed.ItemCategory, 30, goldenSeed.MaxQuantity, 0, 0, -1, goldenSeed.EventID));
            hook.GetItem(new(sacredTear.ID, sacredTear.ItemCategory, 12, sacredTear.MaxQuantity, 0, 0, -1, sacredTear.EventID));
        }

        private void TaliBtn_Click(object sender, EventArgs e)
        {
            if (!hook.Hooked || !hook.Loaded)
                return;
            ItemCategory upgradeMats = ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials");
            Item tali = upgradeMats.Items.FirstOrDefault(x => x.Name == "Talisman Pouch");

            hook.GetItem(new(tali.ID, tali.ItemCategory, 3, tali.MaxQuantity, 0, 0, -1, tali.EventID));
        }

        private void MemoryStonesBtn_Click(object sender, EventArgs e)
        {
            if (!hook.Hooked || !hook.Loaded)
                return;
            ItemCategory upgradeMats = ItemCategory.All.FirstOrDefault(x => x.Name == "Upgrade Materials");
            Item stone = upgradeMats.Items.FirstOrDefault(x => x.Name == "Memory Stone");

            hook.GetItem(new(stone.ID, stone.ItemCategory, 8, stone.MaxQuantity, 0, 0, -1, stone.EventID));
        }

        private void AllMapsBtn_Click(object sender, EventArgs e)
        {
            if (!hook.Hooked || !hook.Loaded)
                return;

            foreach (int flag in mapEventFlagIDs)
            {
                hook.SetEventFlag(flag, true);
            }

            foreach(Continent continent in Continent.All)
            {
                foreach(Hub hub in continent.Hubs)
                {
                    foreach(Grace grace in hub.Graces)
                    {
                        hook.SetEventFlag(grace.EventFlagID, true);
                    }
                }
            }
        }
        private BuildPrefabMaker maker;
        private void SaveBuildBtn_Click(object sender, EventArgs e)
        {
            if (maker == null)
            {
                maker = new(hook, logger);
                maker.Show();
            }
            else
            {
                maker.Close();
                maker = new(hook, logger);
                maker.Show();
            }
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            List<BuildPrefab> builds = BuildSaver.getBuilds();
            BuildPrefabBox.Items.Clear();
            if (builds.Count > 0)
            {
                foreach(BuildPrefab build in builds)
                {
                    BuildPrefabBox.Items.Add(build);
                }
            }
            Blacklist.Initialize();
        }

        private void SetupBuildBtn_Click(object sender, EventArgs e)
        {
            BuildPrefab build = (BuildPrefab)BuildPrefabBox.SelectedItem;
            if (build == null)
                return;
            
            if (build.weapons.Count > 0)
            {
                List<ItemSpawnInfo> weapons = new();
                foreach (WeaponPrefab weapon in build.weapons)
                {
                    ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Items.FirstOrDefault(x => x.ID == weapon.ID && x is Weapon) != null);
                    if (category != null)
                    {
                        Item item = category.Items.FirstOrDefault(x => x.ID == weapon.ID);
                        weapons.Add(new(item.ID, item.ItemCategory, 1, item.MaxQuantity, weapon.Infusion, weapon.UpgradeLevel, weapon.SwordArtID, item.EventID));
                    }
                }
                hook.GetItem(weapons, CancellationToken.None);
            }
            if (build.armors.Count > 0)
            {
                List<ItemSpawnInfo> armors = new();
                foreach (ArmorPrefab armor in build.armors)
                {
                    ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Armor");
                    if (category != null)
                    {
                        Item item = category.Items.FirstOrDefault(x => x.ID == armor.ID);
                        armors.Add(new(item.ID, item.ItemCategory, 1, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID));
                    }
                }
                hook.GetItem(armors, CancellationToken.None);
            }
            if (build.talismans.Count > 0)
            {
                List<ItemSpawnInfo> talismans = new();
                foreach (TalismanPrefab talisman in build.talismans)
                {
                    ItemCategory category = ItemCategory.All.FirstOrDefault(x => x.Name == "Talismans");
                    if (category != null)
                    {
                        Item item = category.Items.FirstOrDefault(x => x.ID == talisman.ID);
                        talismans.Add(new(item.ID, item.ItemCategory, 1, item.MaxQuantity, (int)Infusion.Standard, 0, -1, item.EventID));
                    }
                }
                hook.GetItem(talismans, CancellationToken.None);
            }
        }

        private void PreviewBuildBtn_Click(object sender, EventArgs e)
        {
            BuildPrefab prefab = BuildPrefabBox.SelectedItem as BuildPrefab;
            if (prefab == null)
                return;

            if (maker != null)
            {
                maker.Close();
                maker = null;
            }

            maker = new(hook, logger);
            maker.Show();

            maker.weaponsPrefab = prefab.weapons;
            maker.armorsPrefab = prefab.armors;
            maker.talismanPrefab = prefab.talismans;

            maker.LoadBuild(prefab.BuildName);
        }

        private void ExportBuildBtn_Click(object sender, EventArgs e)
        {
            BuildPrefab prefab = BuildPrefabBox.SelectedItem as BuildPrefab;
            if (prefab == null)
                return;

            FolderBrowserDialog dlg = new();
            dlg.Description = "Select folder to export to.";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (!string.IsNullOrWhiteSpace(dlg.SelectedPath))
            {
                BuildSaver.exportBuild(prefab, dlg.SelectedPath);
            }
        }

        private void ImportBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new();
            fileDialog.Filter = "json files (*.json)|*.json";
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            fileDialog.Multiselect = true;

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            string PathToCopy = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Elden Ring PvPHelper/Builds/");
            foreach (var path in fileDialog.FileNames)
            {
                FileInfo fi = new(path);

                File.Copy(fi.FullName, PathToCopy + fi.Name);
            }

            RefreshBtn.PerformClick();
        }

        MassItemGib massItemGib;
        private void button2_Click(object sender, EventArgs e)
        {
            if (massItemGib != null)
            {
                massItemGib.Close();
                massItemGib = null;
            }

            massItemGib = new(hook);
            massItemGib.Show();
        }

        private void MaxRunesToggle_CheckedChanged(object sender, EventArgs e)
        {
            int currRunes = player.HeldRunes;
            int max = 999999999;

            RunesBox.Value = max - currRunes;
            RunesBox.Maximum = max - currRunes;
        }

        private void RunesBox_ValueChanged(object sender, EventArgs e)
        {
            int currRunes = player.HeldRunes;
            int max = 999999999;
            if (MaxRunesToggle.Checked)
            {
                RunesBox.Value = max - currRunes;
                return;
            }
            RunesBox.Maximum = max - currRunes;
        }
        private void MaxRuneBtn_Click(object sender, EventArgs e)
        {
            if (!hook.Hooked)
            {
                logger.Log("Elden Ring is not Attached.", Logger.LogType.Error);
                return;
            }
            if (!hook.Loaded)
            {
                logger.Log("Player not detected.");
                return;
            }

            hook.AddRunes((int)RunesBox.Value);
        }

        private void AllInvasionZonesBtn_Click(object sender, EventArgs e)
        {
            logger.Log("Sorry, Invasion zones are not implemented yet.", Logger.LogType.Warning);
        }
    }

}
