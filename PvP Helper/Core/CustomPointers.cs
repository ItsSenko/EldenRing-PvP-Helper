using Erd_Tools;
using PropertyHook;

namespace PvPHelper.Core
{
    public class CustomPointers
    {
        private static string ChrDbgFlagsAOB = "80 3D ?? ?? ?? ?? 00 0F 85 ?? ?? ?? ?? 32 C0 48";
        private static string FieldAreaAOB = "48 8B 3D ? ? ? ? 49 8B D8 48 8B F2 4C 8B F1 48 85 FF";
        private static string FieldArea2AOB = "48 8B 0D ?? ?? ?? ?? 48 ?? ?? ?? 44 0F B6 61 ?? E8 ?? ?? ?? ?? 48 63 87 ?? ?? ?? ?? 48 ?? ?? ?? 48 85 C0";
        private static string CSFlipperAOB = "48 8B 0D ?? ?? ?? ?? 80 BB D7 00 00 00 00 0F 84 CE 00 00 00 48 85 C9 75 2E";
        private static string dHitboxAOB = "0F 29 74 24 40 0F 1F";
        private static string SessionManAOB = "4C 8B 05 ?? ?? ?? ?? 48 8B D9 33 C9 0F 29 74 24 ?? 0F 29 7C 24 ?? B2 01";
        private static string GlobalGameDataManAOB = "48 8B 05 ?? ?? ?? ?? 48 85 C0 74 05 48 8B 40 58 C3 C3";
        private const string GetEquipParamGoodsFuncAOB = "40 57 48 83 ec 40 48 c7 44 24 20 fe ff ff ff 48 89 5c 24 50 48 89 6c 24 58 48 89 74 24 60 8b fa 48 8b f1 33 db 85 d2 0f 88 27 01 00 00 48 8b 0d d4 85 04 03 48 85 c9 75 2e 48 8d 0d 38 15 02 03 ?? ?? ?? ?? ?? 4c 8b c8 4c 8d 05 69 91 55 02 ba b4 00 00 00 48 8d 0d 45 e1 c8 01 ?? ?? ?? ?? ?? 48 8b 0d a1 85 04 03";

        public static PHPointer ChrDbgFlags;
        public static PHPointer ChrFlags;
        public static PHPointer idleAnimation;
        public static PHPointer animPointer;

        public static PHPointer FieldArea;
        public static PHPointer FieldArea2;
        public static PHPointer CSFlipper;
        public static PHPointer dHitbox;
        public static PHPointer GlobalSessionMan;
        public static PHPointer SessionMan;
        public static PHPointer GlobalGameDataMan;
        public static PHPointer CSMenuMan;
        public static PHPointer GetEquipParamGoodsFunc;
        public static void Initialize(ErdHook hook)
        {
            ChrDbgFlags = hook.RegisterRelativeAOB(ChrDbgFlagsAOB, 2, 7);

            ChrFlags = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x0 });
            idleAnimation = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x58 });
            animPointer = hook.CreateChildPointer(hook.PlayerIns, new int[] { 0x190, 0x80 });

            FieldArea = hook.RegisterAbsoluteAOB(FieldAreaAOB);
            FieldArea2 = hook.RegisterRelativeAOB(FieldArea2AOB, 3, 7, 0);
            CSFlipper = hook.RegisterRelativeAOB(CSFlipperAOB, 3, 7, 0);
            dHitbox = hook.RegisterRelativeAOB(dHitboxAOB, 12, 16, 0);
            GlobalSessionMan = hook.RegisterRelativeAOB(SessionManAOB, 3, 7);
            SessionMan = hook.RegisterRelativeAOB(SessionManAOB, 3, 7, 0);
            GlobalGameDataMan = hook.RegisterRelativeAOB(GlobalGameDataManAOB, 3, 7);

            CSMenuMan = hook.RegisterRelativeAOB("48 8B 05 ?? ?? ?? ?? 33 DB 48 89 74 24", 3, 7, 0);
            GetEquipParamGoodsFunc = hook.RegisterAbsoluteAOB(GetEquipParamGoodsFuncAOB.ToUpper());
        }
    }
}
