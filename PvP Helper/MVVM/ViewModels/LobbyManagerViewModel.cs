using Erd_Tools;
using PvPHelper.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using PvPHelper.MVVM.Models;
using Erd_Tools.Models;
using static Erd_Tools.Models.Weapon;
using PvPHelper.MVVM.Views.UserControls;
using System.Collections.ObjectModel;
using PvPHelper.MVVM.Commands.PrefabCreator;
using System.Windows.Media;
using PvPHelper.MVVM.Dialogs;
using PropertyHook;
using System.Windows.Threading;
using System;
using System.Windows;
using System.Threading;

namespace PvPHelper.MVVM.ViewModels
{
    public class LobbyManagerViewModel : ViewModelBase
    {
        #region Data Bindings
        private ObservableCollection<NetPlayerControl> _lobbyItemsSource;

        public ObservableCollection<NetPlayerControl> LobbyItemsSource
        {
            get { return _lobbyItemsSource; }
            set { _lobbyItemsSource = value; OnPropertyChanged(); }
        }
        private string _lobbyTitle;

        public string LobbyTitle
        {
            get { return _lobbyTitle; }
            set { _lobbyTitle = value; OnPropertyChanged(); }
        }

        #endregion
        private ErdHook Hook { get; set; }
        private PHPointer Session { get; set; }

        
        public static PHPointer KickOutFunction { get; set; }

        private NetPlayer LocalPlayer { get; set; }
        private NetPlayer Player1 { get; set; }
        private NetPlayer Player2 { get; set; }
        private NetPlayer Player3 { get; set; }
        private NetPlayer Player4 { get; set; }
        private NetPlayer Player5 { get; set; }

        public static List<NetPlayer> PlayerList { get; set; }

        private DispatcherTimer UpdateTimer = new();
        public LobbyManagerViewModel(ErdHook hook)
        {
            Hook = hook;
            Session = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8 });

            LocalPlayer = new(hook, Session, 0x0 * 10);
            Player1 = new(hook, Session, 0x10);
            Player2 = new(hook, Session, 0x20);
            Player3 = new(hook, Session, 0x30);
            Player4 = new(hook, Session, 0x40);
            Player5 = new(hook, Session, 0x50);

            PlayerList = new();
            PlayerList.Add(LocalPlayer);
            PlayerList.Add(Player1);
            PlayerList.Add(Player2);
            PlayerList.Add(Player3);
            PlayerList.Add(Player4);
            PlayerList.Add(Player5);

            UpdateTimer.Interval = TimeSpan.FromSeconds(1);
            UpdateTimer.Tick += UpdateTimer_Tick;

            Hook.OnSetup += Hook_OnSetup;
            LobbyTitle = "Not in a session";
        }

        private void Hook_OnSetup(object? sender, PHEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateTimer.Start();
            });
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (!Hook.Hooked)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    UpdateTimer.Stop();
                });
                return;
            }

            if (!Hook.Loaded)
                return;

            if (PlayerList.FirstOrDefault(x => x.Name != "" && x.Name != LocalPlayer.Name) != null)
            {
                if (LobbyItemsSource == null)
                    LobbyItemsSource = new();

                int playersInLobby = 0;
                foreach(NetPlayer player in PlayerList)
                {
                    if (!string.IsNullOrEmpty(player.Name) && player != LocalPlayer)
                    {
                        playersInLobby++;
                    }
                    if (LobbyItemsSource.FirstOrDefault(x => x.Player == player) != null)
                    {
                        NetPlayerControl control = LobbyItemsSource.FirstOrDefault(x => x.Player == player);

                        if (string.IsNullOrEmpty(player.Name))
                        {
                            LobbyItemsSource.Remove(control);
                        }
                        else
                            control.UpdateUI();

                        continue;
                    }

                    if (!string.IsNullOrEmpty(player.Name) && player != LocalPlayer)
                    {
                        NetPlayerControl newPlayer = new();
                        newPlayer.Setup(player, LocalPlayer, Hook);
                        LobbyItemsSource.Add(newPlayer);
                    }
                }

                LobbyTitle = $"{playersInLobby} {(playersInLobby > 1 ? "Players" : "Player")} in the Lobby";
                return;
            }
            LobbyTitle = "Not in a session";
            if (LobbyItemsSource != null)
            {
                    LobbyItemsSource.Clear();
            }
        }
    }
}
