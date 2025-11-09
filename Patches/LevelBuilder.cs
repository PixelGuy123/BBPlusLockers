using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace BBPlusLockers.Patches;

[HarmonyPatch(typeof(LevelBuilder))]
static class LevelBuilderPatch
{
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(LevelBuilder.LoadRoom), [typeof(RoomAsset), typeof(IntVector2), typeof(IntVector2), typeof(Direction), typeof(bool), typeof(Texture2D), typeof(Texture2D), typeof(Texture2D)])]
    static IEnumerable<CodeInstruction> GetPrefabFromTransforms(IEnumerable<CodeInstruction> i, MethodBody original) // yay, harmony can give method body
    {
        var v11transform = (byte)original.LocalVariables.First(loc => loc.LocalType == typeof(Transform)).LocalIndex; // First one will be available already

        return new CodeMatcher(i)
        .MatchForward(
            true,
            new(OpCodes.Ldloc_S, v11transform),
            new(OpCodes.Ldloc_0),
            new(CodeInstruction.LoadField(typeof(RoomController), "objectObject")),
            new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(GameObject), "transform")),
            new(CodeInstruction.Call(typeof(Object), nameof(Object.Instantiate), [typeof(Transform)], [typeof(Transform)]))
        )
        .Advance(1)
        .InsertAndAdvance(
            new(OpCodes.Dup), // Get the transform2 through stack duplication
            new(OpCodes.Ldloc_S, v11transform), // Get the transform through localVariable reference
            Transpilers.EmitDelegate<System.Action<Transform, Transform>>((clone, prefab) =>
            {
                // TODO: check if the prefab is equal to an existent locker prefab, inside a hashset; if so, add the clone to the list of gameObjects
            })
        )
        .InstructionEnumeration();
    }
}