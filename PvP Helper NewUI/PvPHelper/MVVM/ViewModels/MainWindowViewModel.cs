using Erd_Tools;
using PvPHelper.Core;
using System.Collections.Generic;

namespace PvPHelper.MVVM.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region RelayCommands
        public RelayCommand DashboardCommand { get; set; }
        public RelayCommand ItemsCommand { get; set; }
        public RelayCommand PrefabCreatorCommand { get; set; }
        public RelayCommand YourPrefabsCommand { get; set; }
        public RelayCommand MiscCommand { get; set; }
        #endregion
        private Dictionary<string, ViewModelBase> _viewModels = new();
        private object _currentView;

        private ErdHook _hook;
        public bool HookCompletelyLoaded 
        {
            get { return _hook is not null && _hook.Hooked && _hook.Loaded; }
        }
        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value; 
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            _hook = new(5000, 1000, new(x => x.MainWindowTitle is "ELDEN RING™" or "ELDEN RING"));
            SetupViewModels();
        }

        private void SetupViewModels()
        {
            DashboardCommand = new(o => 
            {
                CurrentView = _viewModels["DashboardView"];
            });
            ItemsCommand = new(o => 
            {
                CurrentView = _viewModels["ItemsView"];
            });
            PrefabCreatorCommand = new(o => 
            {
                CurrentView = _viewModels["PrefabCreatorView"];
            });
            YourPrefabsCommand = new(o => 
            {
                CurrentView = _viewModels["YourPrefabsView"];
            });
            MiscCommand = new(o => 
            {
                CurrentView = _viewModels["MiscView"];
            });

            foreach (var view in _viewModels)
                view.Value.Initialize(_hook);
        }
    }
}
