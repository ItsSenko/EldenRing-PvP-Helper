using Erd_Tools;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PvPHelper.Core
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public virtual void Initialize(ErdHook hook)
        {

        }
    }
}
