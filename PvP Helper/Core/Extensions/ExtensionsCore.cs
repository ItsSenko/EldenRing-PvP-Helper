using Erd_Tools;
using PropertyHook;
using PvPHelper.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PvPHelper.Core.Extensions
{
    public static class ExtensionsCore
    {
        public static void Initialize()
        {
            AddSoul_Call = GetMainHook().RegisterAbsoluteAOB(AddSoul_CallAoB);
            KickOutFunction = GetMainHook().RegisterRelativeAOB(KickOutFunctionAOB, 1, 5);
        }
        public static ErdHook GetMainHook() => MainWindowViewModel.GetMainHook();
        public const string AddSoul_CallAoB = "44 8B 49 ? 45 33 DB 44 89 5C 24";
        public static PHPointer AddSoul_Call = null;

        private static string KickOutFunctionAOB = "e8 ?? ?? ?? ?? 41 ff c6 48 81 c3 00 01 00 00 48 3b df 0f 85 39 ff ff ff";
        public static PHPointer KickOutFunction = null;

        public static string RemoveSpaces(this string str)
        {
            return str.Replace(" ", string.Empty);
        }

        public static Task DownloadAsync(this BitmapImage bitmapImage)
        {
            var tcs = new TaskCompletionSource<bool>();
            EventHandler downloadCompleted = null;
            downloadCompleted = (s, e) =>
            {
                bitmapImage.DownloadCompleted -= downloadCompleted;
                tcs.SetResult(true);
            };

            bitmapImage.DownloadCompleted += downloadCompleted;

            if (bitmapImage.IsDownloading)
            {
                return tcs.Task;
            }
            else
            {
                // The image is already downloaded
                tcs.SetResult(true);
                return tcs.Task;
            }
        }
    }
}
