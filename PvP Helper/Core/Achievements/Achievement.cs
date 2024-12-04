using Erd_Tools;
using System;
using System.Windows.Threading;

namespace PvPHelper.Core.Achievements
{
    public abstract class Achievement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Achieved { get; }
        public int timerInterval { get; }
        public DispatcherTimer _timer { get; set; }
        public event Action<Achievement> OnAchieved;

        public Achievement(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual void Initialize(ErdHook hook)
        {
            _timer = new();
            _timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            _timer.Tick += (s, e) => OnTick();
        }

        public virtual void OnTick()
        {
            if (Achieved)
                return;
        }
    }
}
