using PvPHelper.MVVM.Dialogs;
using PvPHelper.MVVM.Models;
using PvPHelper.MVVM.Models.Builds;
using PvPHelper.MVVM.ViewModels;
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
            Build prefab = viewModel.CurrentBuild;

            if (viewModel.SelectedBuild != null)
            {
                BuildSaver.saveBuild(prefab);
                InformationDialog info = new($"Updated {prefab.Name}.");
                info.ShowDialog();
            }
            else
            {
                CreateBuildDialog dialog = new();
                dialog.OnSave += (name) =>
                {
                    prefab.Name = name;
                    InputDialog dialog2 = new("Author");
                    dialog2.OnSave += (author) =>
                    {
                        prefab.Author = author;
                        InputDialog dialog3 = new("Description");
                        dialog3.OnSave += (desc) =>
                        {
                            prefab.Description = desc;
                            BuildSaver.saveBuild(prefab);
                            InformationDialog info = new($"Saved {name}.");
                            info.ShowDialog();
                        };
                        dialog3.ShowDialog();
                    };
                    dialog2.ShowDialog();
                };
                dialog.ShowDialog();
            }
        }
    }
}
