using PvPHelper.Core.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Console.Commands
{
    public class AchievementCommand : CommandBase
    {
        public AchievementCommand()
        {
            Name = "Achievement Command";
            Description = "View all Achievements";
            CommandString = "/achievments";
            HasParams = false;
            RequireParams = false;
        }

        protected override void OnTriggerCommand()
        {
            foreach(Achievement achievement in Achievements.Instance.AllAchievements)
            {
                CommandManager.Log("Achievement: " + achievement.Name);
                CommandManager.Log("Description: " + achievement.Description);
                CommandManager.Log("HasAchieved: " + achievement.Achieved);
                CommandManager.Log("");
            }
        }
    }
}
