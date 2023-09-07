using Erd_Tools;
using PropertyHook;

namespace PvPHelper.Core
{
    public class CustomPointers
    {
        private static string ChrDbgFlagsAOB = "80 3D ? ? ? ? 00 0F 85 ? ? ? ? 32 C0 48";
        private static string FieldAreaAOB = "48 8B 3D ? ? ? ? 49 8B D8 48 8B F2 4C 8B F1 48 85 FF";
        private static string CSFlipperAOB = "48 8B 0D ?? ?? ?? ?? 80 BB D7 00 00 00 00 0F 84 CE 00 00 00 48 85 C9 75 2E";
        private static string dHitboxAOB = "0F 29 74 24 40 0F 1F";

        public static PHPointer ChrDbgFlags;
        public static PHPointer ChrFlags;
        public static PHPointer idleAnimation;
        public static PHPointer animPointer;

        public static PHPointer FieldArea;
        public static PHPointer CSFlipper;
        public static PHPointer dHitbox;
        public static void Initialize(ErdHook hook)
        {
            ChrDbgFlags = hook.RegisterRelativeAOB(ChrDbgFlagsAOB, 2, 7, 0);

            ChrFlags = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x0 });
            idleAnimation = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x58 });
            animPointer = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x80 });

            FieldArea = hook.RegisterAbsoluteAOB(FieldAreaAOB);
            CSFlipper = hook.RegisterRelativeAOB(CSFlipperAOB, 3, 7, 0);
            dHitbox = hook.RegisterRelativeAOB(dHitboxAOB, 12, 16, 0);
        }
    }
}
