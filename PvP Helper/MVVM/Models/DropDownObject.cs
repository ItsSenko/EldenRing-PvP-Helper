using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.MVVM.Models
{
    public interface IDropDownObject<T>
    {
        public T Value { get; set; }
        public string Name { get; set; }
    }
    public class DropDownObject<T> : IDropDownObject<T>
    {
        public T Value { get; set; }
        public string Name { get; set; }

        public DropDownObject(T value, string name)
        {
            Value = value;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
