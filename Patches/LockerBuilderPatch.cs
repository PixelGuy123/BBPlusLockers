using BBPlusLockers.Lockers;
using HarmonyLib;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace BBPlusLockers.Patches
{
	[HarmonyPatch(typeof(LockerBuilder))]
	internal class LockerBuilderPatch // new lockers here (reminder: _TextureColor is the property for locker colors, not texture)
	{
		[HarmonyPatch("AddLockers")]
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> GetCustomLockers(IEnumerable<CodeInstruction> i) =>
			new CodeMatcher(i)

			.MatchForward(
				true,
				new(OpCodes.Ldnull),
				new(OpCodes.Stloc_1)
				)
			.Advance(1)
			.GetCodeInstruction(out var instruction) // Gets local variable in the operand of this instruction

			.MatchForward(true, 
				new(OpCodes.Ldarg_1),
				new(OpCodes.Ldloc_1),
				new(OpCodes.Callvirt, AccessTools.Method(typeof(Cell), "AddRenderer", [typeof(Renderer)]))
				)
			.Advance(1)
			.InsertAndAdvance(
				new(OpCodes.Ldloc_1), // local variable for mesh renderer
				new(OpCodes.Ldarg_S, instruction.operand),
				Transpilers.EmitDelegate<System.Action<MeshRenderer, System.Random>>((x, y) =>
				{
					var locker = WeightedSelection<LockerObject>.ControlledRandomSelectionList(LockerCreator.lockers, y);
					if (locker == null)
						return; // Hideable locker has been chosen (lol)
					locker.CreateLocker(x.GetComponent<HideableLocker>());
				})
				)

			.InstructionEnumeration();

		[HarmonyPatch("Build")]
		[HarmonyPrefix]
		static void GetRandomChance(System.Random cRNG, ref float ___hideableChance) =>
			___hideableChance *= (float)cRNG.NextDouble() * 7f;
	}


}
