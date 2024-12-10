using System.Collections;
using UnityEngine;
using MTM101BaldAPI.Components;
using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI;

namespace BBPlusLockers.Lockers
{
	public class AquaLocker : Locker, IItemAcceptor
	{

		public void InsertItem(PlayerManager player, EnvironmentController ec)
		{
			used = true;
			Close(false, true, 66);
			player.RuleBreak("Lockers", 1.5f, 0.8f);
			StartCoroutine(GiveMeSpeed(player, ec));
		}
		public bool ItemFits(Items item) =>
			!used && LockerCreator.CanOpenLocker(item);

		bool used = false;

		IEnumerator GiveMeSpeed(PlayerManager pm, EnvironmentController ec)
		{
			var statModifier = pm.GetMovementStatModifier();
			var modifier = new ValueModifier(2f);
			statModifier.AddModifier("walkSpeed", modifier);
			statModifier.AddModifier("runSpeed", modifier);

			yield return new WaitForSecondsEnvironmentTimescale(ec, 10f);

			statModifier.RemoveModifier(modifier);
		}
			
	}

}
