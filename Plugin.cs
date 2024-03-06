using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SkipCook
{
    [BepInPlugin("dev.mooskyfish.skipcook", "Skip Cook", "1.0")]
    [BepInProcess("Bug Fables.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static readonly string Version = MetadataHelper.GetMetadata(typeof(Plugin)).Version.ToString();
        internal static new ManualLogSource Logger;
        public static bool SkipCookToggle;
        public static CodeMatcher CodeMatcher;
        private Harmony _harmony;
        public void Awake()
        {
            _harmony = new Harmony("dev.mooskyfish.skipcook");
            Logger = base.Logger;
            _harmony.PatchAll();
        }
    }
}
