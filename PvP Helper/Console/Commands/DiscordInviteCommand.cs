using System.Diagnostics;

namespace PvPHelper.Console.Commands
{
    public class DiscordInviteCommand : CommandBase
    {
        public DiscordInviteCommand()
        {
            Name = "Discord Invite Command";
            Description = "Returns an invite link to the Elden Ring PvP Helper discord.";
            CommandString = "/discord";
            HasParams = false;
            RequireParams = false;
        }

        protected override void OnTriggerCommand()
        {
            var ps = new ProcessStartInfo("https://www.discord.gg/VmyGAS24Gf")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }
    }
}
