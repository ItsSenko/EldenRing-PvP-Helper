using Erd_Tools;
using PropertyHook;
using System;
using System.Collections.Generic;

namespace PvPHelper.Core.Achievements.AllAchievements
{
    public class PlayerMetAchievement : Achievement
    {
        public bool Achieved => AchievementList.Default.WildSenko;
        public int timerInterval => 1000;
        public event Action<Achievement> OnAchieved = delegate { };

        private ErdHook hook;
        private PHPointer Session;
        private List<NetPlayer> NetPlayerList = new();
        private long steamId;

        public PlayerMetAchievement(string name, string description, long steamId) : base(name, description)
        {
            this.steamId = steamId;
        }

        public override void Initialize(ErdHook hook)
        {
            base.Initialize(hook);

            this.hook = hook;

            Session = hook.CreateChildPointer(hook.WorldChrMan, new int[] { 0x10EF8 });

            NetPlayerList.Add(new(hook, Session, 0x0 * 10));
            NetPlayerList.Add(new(hook, Session, 0x10));
            NetPlayerList.Add(new(hook, Session, 0x20));
            NetPlayerList.Add(new(hook, Session, 0x30));
            NetPlayerList.Add(new(hook, Session, 0x40));
            NetPlayerList.Add(new(hook, Session, 0x50));

            _timer.Start();
        }

        public override void OnTick()
        {
            base.OnTick();

            if (!hook.Loaded)
                return;

            foreach (NetPlayer player in NetPlayerList)
            {
                if (player.SteamID == steamId)
                    OnAchieved?.Invoke(this);
            }
        }
    }
}
