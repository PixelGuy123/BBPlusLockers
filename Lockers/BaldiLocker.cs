using MTM101BaldAPI;
using System.Collections;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class BaldiLocker : Locker, IClickable<int>
	{
		public void Clicked(int player)
		{
			if (used) return;

			used = true;
			StartCoroutine(!ec.GetBaldi() || Random.value >= 0.15f ? BuzzNoise() : Baldi()); // Really low chance to be useful lol
		}

		public bool ClickableHidden() => used;
		public bool ClickableRequiresNormalHeight() => true;
		public void ClickableSighted(int player) { }
		public void ClickableUnsighted(int player) { }

		IEnumerator BuzzNoise()
		{
			audMan.PlaySingle(aud_troll);
			Close(true, true);
			SetMainTex(baldos[3]); // index 3 should be the buzz
			while (audMan.AnyAudioIsPlaying)
				yield return null;
			Close(true, true);
		}

		IEnumerator Baldi()
		{
			Close(false, true);
			var baldo = ec.GetBaldi();
			baldo.spriteRenderer[0].enabled = false;
			baldo.enabled = false;

			yield return new WaitForSecondsEnvironmentTimescale(ec, 2f);

			SetMainTex(baldos[2]);

			yield return new WaitForSecondsEnvironmentTimescale(ec, 0.85f);

			baldo.Navigator.Entity.Teleport(ec.CellFromPosition(transform.position).FloorWorldPosition);
			baldo.enabled = true;
			baldo.TakeApple();
			baldo.spriteRenderer[0].enabled = true;

			Close(true, true);
		}

		

		bool used = false;

		internal static Texture2D[] baldos;
	}
}
