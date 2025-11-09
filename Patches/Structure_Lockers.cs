using System.Collections.Generic;
using System.Reflection.Emit;
using BBPlusLockers.Structures;
using HarmonyLib;
using UnityEngine;

namespace BBPlusLockers.Patches;

[HarmonyPatch(typeof(Structure_Lockers))]
static class Structure_Lockers_Patch
{
    [HarmonyPatch(nameof(Structure_Lockers.AddLockers))]
    [HarmonyTranspiler]
    static IEnumerable<CodeInstruction> FindMeshRendererAndRegisterIt(IEnumerable<CodeInstruction> i) =>
        new CodeMatcher(i)
        .End() // Goes to the end
        .InsertAndAdvance(
            CodeInstruction.LoadField(typeof(Structure_CustomLockers), nameof(Structure_CustomLockers.replaceableLockers)), // Loads the list into the stack
            new CodeInstruction(OpCodes.Ldloc_1),                                                                           // Loads mesh renderer into the stack
            CodeInstruction.Call(typeof(MeshRenderer), nameof(MeshRenderer.gameObject)),                                    // Get the gameObject
            CodeInstruction.Call(typeof(List<GameObject>), nameof(List<>.Add))                                              // Adds the gameObject inside the list
        )
        .InstructionEnumeration();
}