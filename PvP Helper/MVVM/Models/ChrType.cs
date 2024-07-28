using System.Drawing;

namespace PvPHelper.MVVM.Models
{
    public class ChrType : BaseOption
    {
        public int ChrID { get; set; }
        public int ParamID { get; set; }
        public Color diffMulColor { get; set; }
        public Color edgeColor { get; set; }
        public Color frontColor { get; set; }
    }

    public class PhantomIDOption : BaseOption
    {
        public int ID { get; set; }

        public PhantomIDOption(string name, int ID)
        {
            this.Name = name;
            this.ID = ID;
        }

        public PhantomIDOption(int ID)
        {
            this.ID = ID;
            Name = ID.ToString();
        }
    }
}
