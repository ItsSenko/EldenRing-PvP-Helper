using Erd_Tools;
using Erd_Tools.Models;
using PvPHelper.MVVM.Dialogs;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using static Erd_Tools.Models.Param;
using static Erd_Tools.Models.Weapon;
using CommandBase = PvPHelper.Core.CommandBase;

namespace PvPHelper.MVVM.Commands.PrefabCreator
{
    internal class SavePrefab : CommandBase
    {
        private PrefabCreatorViewModel viewModel;
        public SavePrefab(PrefabCreatorViewModel vm)
        {
            viewModel = vm;
        }
        public override void Execute(object? parameter)
        {
            BuildPrefab prefab;

            if (viewModel.SelectedBuild != null)
            {
                if (viewModel.weaponPrefabs.Count == 0 && viewModel.armorPrefabs.Count == 0 && viewModel.talismanPrefabs.Count == 0)
                    return;
                prefab = viewModel.SelectedBuild as BuildPrefab;
                prefab.weapons = viewModel.weaponPrefabs;
                prefab.armors = viewModel.armorPrefabs;
                prefab.talismans = viewModel.talismanPrefabs;

                BuildSaver.saveBuild(prefab);
                viewModel.RefreshBuilds.Execute(prefab);
                InformationDialog info = new($"Updated {prefab.BuildName}.");
                info.ShowDialog();
            }
            else
            {
                if (viewModel.weaponPrefabs.Count == 0 && viewModel.armorPrefabs.Count == 0 && viewModel.talismanPrefabs.Count == 0)
                    return;
                CreateBuildDialog dialog = new();
                dialog.OnSave += (name) => 
                {
                    prefab = new(name, viewModel.weaponPrefabs, viewModel.armorPrefabs, viewModel.talismanPrefabs);
                    BuildSaver.saveBuild(prefab);

                    InformationDialog info = new($"Saved {name}.");
                };
                dialog.ShowDialog();
            }
        }
    }
}
