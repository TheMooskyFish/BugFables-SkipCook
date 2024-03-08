using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace SkipCook.Patches
{
    public class Kut
    {
        public static void Patch()
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
            Utils.MatchThenNopify(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name.StartsWith("<t>")),
                new CodeMatch(OpCodes.Callvirt),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(GameObject), "Destroy", [typeof(Object)]))
            );
        }
    }
}
