using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PvPHelper.Core;
using System.Reflection;

namespace PvPHelper.MVVM.Models.Regions
{
    public class PlayRegion
    {
        public string Map { get; set; }
        public string Name { get; set; }
        public int PlayRegionID { get; set; }
        public int[] BonfireFlags { get; set; }
        public bool IsOpenWorld { get; set; }
        public bool IsDungeon { get; set; }
        public bool IsBoss { get; set; }
        public bool IsActive { get; set; }
        public PlayRegion(string mapName, string name, int playregion, int[] bonfireFlags, bool isOpenWorld = false, bool isDungeon = false, bool isBoss = false, bool isActive = false)
        {
            Map = mapName;
            Name = name;
            PlayRegionID = playregion;
            BonfireFlags = bonfireFlags;
            IsOpenWorld = isOpenWorld;
            IsDungeon = isDungeon;
            IsBoss = isBoss;
            IsActive = isActive;
        }
    }
}
