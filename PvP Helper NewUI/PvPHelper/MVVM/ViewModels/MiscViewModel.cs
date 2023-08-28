using Erd_Tools;
using Erd_Tools.Models.Entities;
using PvPHelper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommandManager = PvPHelper.Console.CommandManager;
using PvPHelper.MVVM.Commands.Dashboard.Toggles;
using System.Windows.Threading;
using PvPHelper.MVVM.Models;
using System.Drawing;
using PvPHelper.MVVM.Views;
using System.Diagnostics;
using PvPHelper.MVVM.Commands.Misc;

namespace PvPHelper.MVVM.ViewModels
{
    public class MiscViewModel : ViewModelBase
    {
        #region DataBindings
        public ShowHitboxesToggle ShowHitboxesToggle { get; set; }
        public AutoUpdateToggle AutoUpdateToggle { get; set; }
        public CheckForUpdates CheckForUpdates { get; set; }
        #endregion
        private ErdHook hook;
        public MiscViewModel(ErdHook hook, VersionController versionController)
        {
            this.hook = hook;

            ShowHitboxesToggle = new(hook);
            AutoUpdateToggle = new();
            CheckForUpdates = new(versionController);
        }
    }
}
