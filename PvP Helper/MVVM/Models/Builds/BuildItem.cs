using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Builds
{
    public class BuildItem
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public short IconID { get; set; }
        public string Category { get; set; }

        public BuildItem(string name, int iD, short iconID, string category)
        {
            Name = name;
            ID = iD;
            IconID = iconID;
            Category = category;
        }
    }
}
