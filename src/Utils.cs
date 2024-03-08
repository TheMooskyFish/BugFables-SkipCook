using HarmonyLib;
using System.Reflection.Emit;

namespace SkipCook
{
    public static class Utils
    {
        public static void RemoveEventControlWait(string waitForSeconds)
        {
            Plugin.CodeMatcher.MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(typeof(EventControl), waitForSeconds)),
                new CodeMatch(OpCodes.Stfld)
            ).ThrowIfInvalid("Can't find");
            Nopify(2);
        }
        public static void RemoveWaitForSeconds(float secs)
        {
            Plugin.CodeMatcher.MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldc_R4, secs),
                new CodeMatch(OpCodes.Newobj)
            ).ThrowIfInvalid("Can't find");
            Nopify(3);
        }
        public static void Nopify(int instsnumber)
        {
            foreach (var _ in Plugin.CodeMatcher.InstructionsWithOffsets(0, instsnumber))
            {
                Plugin.CodeMatcher.SetAndAdvance(OpCodes.Nop, null);
            }
        }
        public static void MatchThenNopify(bool useEnd, params CodeMatch[] codeMatches)
        {
            var oldPos = Plugin.CodeMatcher.Pos;
            var currentPos = Plugin.CodeMatcher.MatchForward(useEnd, codeMatches).Advance(useEnd ? 0 : -1).Pos;
            Plugin.CodeMatcher.Advance(oldPos - currentPos);
            Nopify(currentPos - oldPos);
        }
        public static void DebugPrint(int start, int end)
        {
            foreach (var i in Plugin.CodeMatcher.InstructionsWithOffsets(start, end))
            {
                Plugin.Logger.LogInfo(i);
            }
        }
    }
}
