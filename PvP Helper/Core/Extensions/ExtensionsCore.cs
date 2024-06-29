using Erd_Tools;
using PropertyHook;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPHelper.Core.Extensions
{
    public static class ExtensionsCore
    {
        public static void Initialize()
        {
            AddSoul_Call = GetMainHook().RegisterAbsoluteAOB(AddSoul_CallAoB);
        }
        public static ErdHook GetMainHook() => MainWindowViewModel.GetMainHook();
        public const string AddSoul_CallAoB = "44 8B 49 ? 45 33 DB 44 89 5C 24";
        public static PHPointer AddSoul_Call = null;

        public static string RemoveSpaces(this string str)
        {
            return str.Replace(" ", string.Empty);
        }
    }
}
