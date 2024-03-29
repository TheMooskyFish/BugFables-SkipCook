﻿using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace SkipCook.Patches
{
    public class Fry
    {
        public static void Patch()
        {
            Plugin.Logger.LogInfo("Patching Fry's code path");
            var item = Plugin.CodeMatcher.Instructions().First(i => i.opcode == OpCodes.Ldfld && ((FieldInfo)i.operand).Name.StartsWith("<t>"));
            Plugin.CodeMatcher.Start()
            .MatchForward(true,
                new CodeMatch(OpCodes.Call),
                new CodeMatch(OpCodes.Stfld, item.operand)
            ).Advance(1).InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(item),
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Call, AccessTools.PropertySetter(typeof(SpriteRenderer), nameof(SpriteRenderer.enabled)))
            ).Advance(3);
            Utils.MatchThenNopify(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldfld),
                new CodeMatch(OpCodes.Callvirt),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(GameObject), "Destroy", [typeof(Object)]))
            );
            Plugin.CodeMatcher.Advance(4);
            Utils.MatchThenNopify(true,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldfld),
                new CodeMatch(OpCodes.Ldc_R4),
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(GameObject), "Destroy", [typeof(Object), typeof(float)]))
            );
        }
    }
}
