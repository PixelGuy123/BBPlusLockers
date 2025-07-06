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
			if (NYANCATCor != null)
			{
				pm.Am.moveMods.Remove(moveMod);
				StopCoroutine(NYANCATCor);
			}
			pm.Am.moveMods.Add(moveMod);
			NYANCATCor = StartCoroutine(NYANCAT(pm));
		}

		protected override void AfterTrollAndClose(PlayerManager pm)
		{
			base.AfterTrollAndClose(pm);
			if (NYANCATCor != null)
			{
				pm.Am.moveMods.Remove(moveMod);
				StopCoroutine(NYANCATCor);
			}
		}

		IEnumerator NYANCAT(PlayerManager pm)
		{
			yield return null;
			var cam = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber);
			while (opened)
			{
				moveMod.movementAddend = cam.transform.forward * backwardsSpeed;
				yield return null;
			}

		}

		readonly MovementModifier moveMod = new(Vector3.zero, 0f);
		AudioManager nyanAudMan;
		Coroutine NYANCATCor;
		internal float backwardsSpeed = -45f;
	}
}
