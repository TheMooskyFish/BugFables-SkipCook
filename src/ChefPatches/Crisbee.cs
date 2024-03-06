using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace SkipCook.Patches
{
    public class Crisbee
    {
        public static void Patch()
        {
            try
            {
                Plugin.Logger.LogInfo("Patching Crisbee's code path");
                Plugin.CodeMatcher.Start()
                .MatchForward(false,
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(Transform), nameof(Transform.position))),
                    new CodeMatch(OpCodes.Ldsfld),
                    new CodeMatch(OpCodes.Ldfld)
                ).Advance(1);
                Utils.Nopify(29);
                Plugin.CodeMatcher.MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_R4, 60f)
                ).SetOperandAndAdvance(-1f).Advance(1);
                Utils.Nopify(13);
                Plugin.CodeMatcher.MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_R4, 1.7f),
                    new CodeMatch(OpCodes.Newobj),
                    new CodeMatch(OpCodes.Stfld)
                ).Advance(-1);
                Utils.Nopify(31);
                Plugin.CodeMatcher.MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_I4_S),
                    new CodeMatch(OpCodes.Stfld),
                    new CodeMatch(OpCodes.Br),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldc_R4, 0.75f)
                );
                Utils.RemoveWaitForSeconds(0.75f);
                //MISTAKE EVENT
                Plugin.CodeMatcher.MatchBack(false,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Brtrue),
                    new CodeMatch(OpCodes.Nop)
                );
                Utils.RemoveEventControlWait("halfsec");
                for (var i = 0; i < 2; i++)
                {
                    Plugin.CodeMatcher.MatchForward(true,
                        new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(typeof(EventControl), "call")),
                        new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(NPCControl), "entity")),
                        new CodeMatch(OpCodes.Ldc_I4_S)
                    ).SetOperandAndAdvance(6);
                }
            }
            catch (System.Exception msg)
            {
                Plugin.Logger.LogError(msg);
            }
        }
    }
}
