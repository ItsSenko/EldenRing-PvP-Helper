using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models
{
    public class BaseOption
    {
        public string Name { get; set; }
        public BaseOption(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
