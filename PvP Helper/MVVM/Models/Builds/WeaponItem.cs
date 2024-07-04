using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models.Builds
{
    public class WeaponItem : BuildItem
    {
        public int Infusion { get; set; }
        public string InfusionIconID { get; set; }

        public int SwordArtID { get; set; }
        public short GemIconID { get; set; }

        public int UpgradeLevel { get; set; }

        public WeaponItem(string name, int iD, short iconID, string category, int infusion, string infusionIconID, int swordArtID, short gemIconID, int upgradeLevel) : base(name, iD, iconID, category)
        {
            Infusion = infusion;
            InfusionIconID = infusionIconID;
            SwordArtID = swordArtID;
            GemIconID = gemIconID;
            UpgradeLevel = upgradeLevel;
        }
    }
}
