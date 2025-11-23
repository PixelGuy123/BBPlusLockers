using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BBPlusLockers.Plugin;
using BBPlusLockers.Structures;
using HarmonyLib;
using UnityEngine;

namespace BBPlusLockers.Patches;

[HarmonyPatch(typeof(LevelBuilder))]
static class LevelBuilderPatch
{
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(LevelBuilder.LoadRoom), [typeof(RoomAsset), typeof(IntVector2), typeof(IntVector2), typeof(Direction), typeof(bool), typeof(Texture2D), typeof(Texture2D), typeof(Texture2D)])]
    static IEnumerable<CodeInstruction> GetPrefabFromTransforms(IEnumerable<CodeInstruction> i, MethodBase method) // yay, harmony can give method body
    {
        var original = method.GetMethodBody();
        var v11transform = (byte)original.LocalVariables.First(loc => loc.LocalType == typeof(Transform)).LocalIndex; // First one will be available already
        var v12envObj = (byte)original.LocalVariables.First(loc => loc.LocalType == typeof(EnvironmentObject)).LocalIndex;

        return new CodeMatcher(i)
        .MatchForward(
            true,
            new(OpCodes.Callvirt, AccessTools.Method(typeof(Component), "GetComponent", generics: [typeof(EnvironmentObject)])),
            new(OpCodes.Stloc_S, v12envObj)
        )
        .Advance(1)
        .InsertAndAdvance( // Delegate(component.GetComponent<Transform>(), transform2)
            new(OpCodes.Ldloc_S, v12envObj),                                                                             // Get the EnvironmentObject from the transform2
            new(OpCodes.Callvirt, AccessTools.Method(typeof(Component), "GetComponent", generics: [typeof(Transform)])), // Get the transform from the EnvironmentObject
            new(OpCodes.Ldloc_S, v11transform),                                                                          // Get the transform through localVariable reference
            Transpilers.EmitDelegate<System.Action<Transform, Transform>>((clone, prefab) =>
            {
                if (ExtraLockersPlugin.lockerPrefabs.Contains(prefab)) // If the prefab is a known locker, add it to the lockers list
                    Structure_CustomLockers.replaceableLockers.Add(clone.gameObject);
            })
        )
        .InstructionEnumeration();
    }
}