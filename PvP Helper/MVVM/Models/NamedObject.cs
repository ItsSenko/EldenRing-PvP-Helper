using static Erd_Tools.Models.Weapon;

namespace PvPHelper.MVVM.Models
{
    public class NamedObject<T>
    {
        public T Value { get; set; }
        public string Name { get; set; }

        public NamedObject(T value, string name)
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
