using System.Collections;
using MTM101BaldAPI;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class BaldiLocker : Locker, IItemAcceptor
	{
		public void InsertItem(PlayerManager player, EnvironmentController ec)
		{
			if (used) return;

			used = true;
			player.RuleBreak("Lockers", 1.5f, 0.8f);
			var baldi = ec.GetBaldi();
			StartCoroutine(
				!baldi ||
				baldi.Navigator.Entity.Frozen ||
				baldi.Navigator.Entity.InBounds ||
				Random.value <= 0.75f ? // 75% of Buzz, 25% of Baldi
				BuzzNoise() : Baldi()); // Really low chance to be useful lol
		}
		public bool ItemFits(Items item) =>
			!used && LockerCreator.CanOpenLocker(item);

		IEnumerator BuzzNoise()
		{
			audMan.PlaySingle(aud_troll);
			Close(true, true, 35);
			SetMainTex(baldos[3]); // index 3 should be the buzz

			while (audMan.AnyAudioIsPlaying)
				yield return null;

			Close(true, true);
		}

		IEnumerator Baldi()
		{
			Close(false, true);
			audMan.PlaySingle(audOhHi);
			var baldo = ec.GetBaldi();
			baldo.spriteRenderer[0].enabled = false;
			baldo.enabled = false;

			yield return new WaitForSecondsEnvironmentTimescale(ec, 2f);

			SetMainTex(baldos[2]);

			yield return new WaitForSecondsEnvironmentTimescale(ec, 0.85f);

			audMan.PlaySingle(audPop);
			baldo.Navigator.Entity.Teleport(ec.CellFromPosition(transform.position).FloorWorldPosition);
			baldo.enabled = true;
			baldo.TakeApple();
			baldo.spriteRenderer[0].enabled = true;

			Close(true, true);
		}



		bool used = false;

		internal static Texture2D[] baldos;

		internal static SoundObject audOhHi, audPop;
	}
}
