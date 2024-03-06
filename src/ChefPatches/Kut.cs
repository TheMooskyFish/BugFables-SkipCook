using System.Reflection.Emit;
using HarmonyLib;

namespace SkipCook.Patches
{
    public class Kut
    {
        public static void Patch()
        {
            try
            {
                Plugin.Logger.LogInfo("Patching Kut's code path");
                Plugin.CodeMatcher.Start()
                .MatchForward(true,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Bne_Un),
                    new CodeMatch(OpCodes.Ldarg_0)
                );
                Utils.Nopify(314);
            }
            catch (System.Exception msg)
            {
                Plugin.Logger.LogError(msg);
            }
        }
    }
}
