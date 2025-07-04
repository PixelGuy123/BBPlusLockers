﻿using System.Collections;
using MTM101BaldAPI;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class BaldiLocker : Locker, IClickable<int>
	{
		public void Clicked(int player)
		{
			if (used) return;

			used = true;
			Singleton<CoreGameManager>.Instance.GetPlayer(player).RuleBreak("Lockers", 1.5f, 0.8f);
			var baldi = ec.GetBaldi();
			StartCoroutine(
				!baldi ||
				baldi.Navigator.Entity.Frozen ||
				baldi.Navigator.Entity.InBounds ||
				Random.value >= 0.15f ?
				BuzzNoise() : Baldi()); // Really low chance to be useful lol
		}

		public bool ClickableHidden() => used;
		public bool ClickableRequiresNormalHeight() => true;
		public void ClickableSighted(int player) { }
		public void ClickableUnsighted(int player) { }

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
