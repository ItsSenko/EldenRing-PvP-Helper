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
}
