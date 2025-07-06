using System.Collections;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace BBPlusLockers.Lockers.DecoyLockers
{
	public class DecoyAquaLocker : AcceptorDecoyLocker
	{
		protected override void AwakeFunc()
		{
			base.AwakeFunc();
			nyanAudMan = gameObject.CreateAudioManager(55f, 100f).MakeAudioManagerNonPositional();
		}
		protected override void ScammedPlayer(PlayerManager pm)
		{
			base.ScammedPlayer(pm);
			audMan.FlushQueue(true);
			nyanAudMan.QueueAudio(aud_troll);
			StartCoroutine(NYANCAT(pm));
		}

		IEnumerator NYANCAT(PlayerManager pm)
		{
			pm.Am.moveMods.Add(moveMod);
			var cam = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber);
			while (opened)
			{
				moveMod.movementAddend = cam.transform.forward * backwardsSpeed;
				yield return null;
			}
			pm.Am.moveMods.Remove(moveMod);
		}

		readonly MovementModifier moveMod = new(Vector3.zero, 0f);
		AudioManager nyanAudMan;
		internal float backwardsSpeed = -23f;
	}
}
