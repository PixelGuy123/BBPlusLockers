using BBPlusLockers.Lockers;
using HarmonyLib;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace BBPlusLockers.Patches
{
	[HarmonyPatch(typeof(Structure_Lockers))]
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
			.GetCodeInstruction(out var instruction) // Gets local variable in the operand of this instruction (cRNG)

			.MatchForward(true, 
				new(OpCodes.Ldarg_0),
				new(CodeInstruction.LoadField(typeof(Structure_Lockers), "lockerPre")), // Looks for red locker, not blue one anymore
				new(OpCodes.Ldarg_1),
				new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Cell), "ObjectBase"))
				)
			.Advance(3)
			.InsertAndAdvance(
				new(OpCodes.Ldloc_1), // local variable for mesh renderer
				new(OpCodes.Ldarg_S, instruction.operand),
				Transpilers.EmitDelegate<System.Action<MeshRenderer, System.Random>>((x, y) =>
				{
					var locker = WeightedSelection<LockerObject>.ControlledRandomSelectionList(LockerCreator.lockers, y);
					if (locker == null)
						return; // Hideable locker has been chosen (lol)
					locker.CreateLocker(x.gameObject);
				})
				)

			.InstructionEnumeration();

		//[HarmonyPatch("Build")]
		//[HarmonyPrefix] Not needed anymore
		//static void GetRandomChance(System.Random cRNG, ref float ___hideableChance) =>
		//	___hideableChance *= (float)cRNG.NextDouble() * 8.4f;
	}


}
