using Erd_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PvPHelper.Core.Achievements
{
    public interface IAchievement
    {
        public string Name { get; }
        public string Description { get; }
        public bool Achieved { get; }
        public int timerInterval { get; }
        public DispatcherTimer _timer { get; set; }
        public void Initialize(ErdHook hook);
        public event Action<IAchievement> OnAchieved;
        
    }
}
