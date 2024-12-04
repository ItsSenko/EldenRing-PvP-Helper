using Erd_Tools;
using PvPHelper.Core.Achievements.AllAchievements;
using PvPHelper.Core.Extensions;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using System.Windows;
using CommandManager = PvPHelper.Console.CommandManager;

namespace PvPHelper.Core.Achievements
{
    public class Achievements
    {
        private static Achievements _instance;
        public static Achievements Instance 
        {
            get 
            { 
                if (_instance == null)
                    _instance = new Achievements();
                return _instance;
            } 
        }
        public List<Achievement> AllAchievements { get; private set; }
        private ErdHook _hook;

        public static void Initialize(ErdHook hook)
        {
            Instance._hook = hook;

            Instance.AllAchievements = new List<Achievement>();
            Instance.AllAchievements.Add(new PlayerMetAchievement("Wild Senko", "Find Senko in a seamless session.", 76561198802512406)); // Senko's SteamID

            foreach (Achievement achievement in Instance.AllAchievements)
            {
                achievement.Initialize(hook);
                achievement.OnAchieved += Instance.OnAchieved;
            }
            ToastNotificationManagerCompat.OnActivated += OnNotificationClicked;
        }

        private static void OnNotificationClicked(ToastNotificationActivatedEventArgsCompat e)
        {
            var args = ToastArguments.Parse(e.Argument);

            if (args["action"] == "focusApp")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = Application.Current.MainWindow;
                    if (mainWindow != null)
                    {
                        if (mainWindow.WindowState == WindowState.Minimized)
                        {
                            mainWindow.WindowState = WindowState.Normal;
                        }
                        mainWindow.Activate();
                        mainWindow.Topmost = true;
                        mainWindow.Topmost = false;
                        mainWindow.Focus();
                    }
                });
            }
        }

        private void OnAchieved(Achievement achievment)
        {
            SetAchievementInList(achievment);

            if (Settings.Default.EnableAchievements)
            {
                SendNotification("You earned an achievement!", $"{achievment.Name} - {achievment.Description}");
                CommandManager.Log("You earned an achievement!");
                CommandManager.Log("Achievement: " + achievment.Name);
                CommandManager.Log("Description: " + achievment.Description);
                CommandManager.Log("You can disable achievements by doing '/settings enableAchievements false'");
            }
        }

        private void SendNotification(string title, string body)
        {
            new ToastContentBuilder()
                .AddAppLogoOverride(new(Path.Combine(Directory.GetCurrentDirectory(), "Resources/Images/shunter.png")))
                .AddText(title)
                .AddText(body)
                .AddArgument("action", "focusApp")
                .Show();
        }

        public static bool GetAchievementInList(Achievement achievement)
        {
            switch (achievement.Name.ToLower().RemoveSpaces())
            {
                case "wildsenko":
                    {
                        return AchievementList.Default.WildSenko;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        public static void SetAchievementInList(Achievement achievement)
        {
            switch (achievement.Name.ToLower().RemoveSpaces())
            {
                case "wildsenko":
                    {
                        AchievementList.Default.WildSenko = true;
                        AchievementList.Default.Save();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
