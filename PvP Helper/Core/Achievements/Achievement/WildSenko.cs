using Erd_Tools;
using PropertyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PvPHelper.Core.Achievements.Achievement
{
    public class WildSenko : IAchievement
    {
        public string Name => "Wild Senko";
        public string Description => "Find Senko in a seamless session";
        public bool Achieved => AchievementList.Default.WildSenko;

        public int timerInterval => 1000;

        public DispatcherTimer _timer { get; set; }

        public event Action<IAchievement> OnAchieved = delegate { };

        private ErdHook hook;
        private PHPointer Session;
        private NetPlayer LocalNetPlayer;
        private NetPlayer NetPlayer1;
        private NetPlayer NetPlayer2;
        private NetPlayer NetPlayer3;
        private NetPlayer NetPlayer4;
        private NetPlayer NetPlayer5;
        private List<NetPlayer> NetPlayerList = new();

        private long senkosSteamID = 76561198802512406;
        public void Initialize(ErdHook hook)
        {
            _timer = new();
            _timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            _timer.Tick += CheckForAchievement;
            this.hook = hook;

            Session = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8 });
            LocalNetPlayer = new(hook, Session, 0x0 * 10);
            NetPlayer1 = new(hook, Session, 0x10);
            NetPlayer2 = new(hook, Session, 0x20);
            NetPlayer3 = new(hook, Session, 0x30);
            NetPlayer4 = new(hook, Session, 0x40);
            NetPlayer5 = new(hook, Session, 0x50);

            NetPlayerList.Add(LocalNetPlayer);
            NetPlayerList.Add(NetPlayer1);
            NetPlayerList.Add(NetPlayer2);
            NetPlayerList.Add(NetPlayer3);
            NetPlayerList.Add(NetPlayer4);
            NetPlayerList.Add(NetPlayer5);

            _timer.Start();
        }

        private void CheckForAchievement(object? sender, EventArgs e)
        {
            if (Achieved || !hook.Loaded)
                return;

            foreach (NetPlayer player in NetPlayerList)
            {
                if (player.SteamID == senkosSteamID)
                    OnAchieved?.Invoke(this);
            }
        }
    }
}
