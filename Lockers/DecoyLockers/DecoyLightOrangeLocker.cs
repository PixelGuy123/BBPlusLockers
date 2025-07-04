using System.Collections;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace BBPlusLockers.Lockers.DecoyLockers
{
	public class DecoyLightOrangeLocker : AcceptorDecoyLocker
	{
		protected override void AfterTrollAndClose(PlayerManager pm)
		{
			base.AfterTrollAndClose(pm);
			SlideTimeScaleTo(1f, true);
		}

		protected override void ScammedPlayer(PlayerManager pm)
		{
			base.ScammedPlayer(pm);
			int points = Singleton<CoreGameManager>.Instance.GetPoints(pm.playerNumber);
			Singleton<CoreGameManager>.Instance.AddPoints(
				-Mathf.Min(points, Random.Range(165, 260)) // Mathf Min to prevent negative points
			, pm.playerNumber,
			true);

			ec.AddTimeScale(timeScale);
			SlideTimeScaleTo(1.25f, false);
			gaugeIcon = gaugeSprite;
		}

		protected override void AwakeFunc()
		{
			base.AwakeFunc();
			audMan.useUnscaledPitch = true; // Avoid getting the pitch up
		}

		protected void SlideTimeScaleTo(float val, bool removeAfterwards)
		{
			if (slideCor != null)
				StopCoroutine(slideCor);
			slideCor = StartCoroutine(SlideTimeScale(val, removeAfterwards));
		}

		IEnumerator SlideTimeScale(float toValue, bool removeAfterwards)
		{
			float slide = timeScale.npcTimeScale;
			while (!slide.CompareFloats(toValue))
			{
				slide += (toValue - slide) * 3.5f * Time.deltaTime;
				timeScale.environmentTimeScale = slide;
				timeScale.npcTimeScale = slide;
				yield return null;
			}
			timeScale.environmentTimeScale = toValue;
			timeScale.npcTimeScale = toValue;

			if (removeAfterwards)
				ec.RemoveTimeScale(timeScale);
		}

		Coroutine slideCor;
		readonly TimeScaleModifier timeScale = new(1f, 1f, 1f);

		internal static Sprite gaugeSprite;
	}
}
