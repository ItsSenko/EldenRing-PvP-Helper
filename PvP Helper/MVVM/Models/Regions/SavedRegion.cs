using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Regions
{
    public class SavedRegion
    {
        public string Name { get; set; }
        public List<BaseRegion> Regions { get; set; }
        public SavedRegion(string name, List<BaseRegion> regions)
        {
            Name = name;
            Regions = regions;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class BaseRegion
    {
        public string Map { get; set; }
        public string Name { get; set; }
        public int PlayRegionID { get; set; }
        public bool IsActive { get; set; }

        public BaseRegion(string map, string name, int id, bool isActive)
        {
            Map = map;
            Name = name;
            PlayRegionID = id;
            IsActive = isActive;
        }
    }
}
