using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace SkipCook.Patches
{
    public class SkipCookPatches
    {
        [HarmonyPatch(typeof(EventControl), nameof(EventControl.Event3), MethodType.Enumerator)]
        class Event3Patch
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Patch(IEnumerable<CodeInstruction> insts, ILGenerator iLGen)
            {
                Plugin.CodeMatcher = new CodeMatcher(insts, iLGen);
                var moveTowardsSkipLabel = iLGen.DefineLabel();
                Plugin.CodeMatcher.MatchForward(false,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Brtrue),
                    new CodeMatch(OpCodes.Ldsfld),
                    new CodeMatch(OpCodes.Ldfld)
                ).Insert(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Plugin), "SkipCookToggle")),
                    new CodeInstruction(OpCodes.Brfalse, moveTowardsSkipLabel)
                ).MatchForward(false,
                    new CodeMatch(OpCodes.Ldsfld),
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(MainManager), "flags"))
                ).Advance(-1).Instruction.labels.Add(moveTowardsSkipLabel);
                Plugin.CodeMatcher.MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_I4_S),
                    new CodeMatch(OpCodes.Stfld, AccessTools.Field(typeof(EntityControl), "animstate"))
                ).Operand = 0;
                Fry.Patch();
                Kut.Patch();
                Crisbee.Patch();
                return Plugin.CodeMatcher.InstructionEnumeration();
            }
        }
    }
}
