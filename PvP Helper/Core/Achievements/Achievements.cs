using Erd_Tools;
using PvPHelper.Core.Achievements.Achievement;
using PvPHelper.Core.Extensions;
using PvPHelper.Console;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
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
        public List<IAchievement> AllAchievements { get; private set; }
        private ErdHook _hook;

        public static void Initialize(ErdHook hook)
        {
            Instance._hook = hook;

            Instance.AllAchievements = new List<IAchievement>();
            Instance.AllAchievements.Add(new WildSenko());

            foreach (IAchievement achievement in Instance.AllAchievements)
            {
                achievement.Initialize(hook);
                achievement.OnAchieved += Instance.OnAchieved;
            }
            ToastNotificationManagerCompat.OnActivated += OnNotificationClicked;
        }

        private static void OnNotificationClicked(ToastNotificationActivatedEventArgsCompat e)
        {
            // Parse the arguments
            var args = ToastArguments.Parse(e.Argument);

            // Check if the action is to focus the app
            if (args["action"] == "focusApp")
            {
                // Bring the main window to the foreground
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
                        mainWindow.Topmost = true;  // Ensure it's on top
                        mainWindow.Topmost = false; // Reset the topmost property
                        mainWindow.Focus();         // Set focus
                    }
                });
            }
        }

        private void OnAchieved(IAchievement achievment)
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

        public static bool GetAchievementInList(IAchievement achievement)
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
        public static void SetAchievementInList(IAchievement achievement)
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
