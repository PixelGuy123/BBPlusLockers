using PixelInternalAPI.Classes;
using System.Collections;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class BlackLocker : Locker // technically a decoy, but it'll be a normal locker because it's random number lol
	{
		protected override void AwakeFunc()
		{
			base.AwakeFunc();

			trigger = new GameObject("BlacklockerTrigger", typeof(BlackLockerTrigger)).AddComponent<BoxCollider>();
			trigger.GetComponent<BlackLockerTrigger>().locker = this;

			trigger.transform.SetParent(transform);
			trigger.transform.localPosition = -transform.forward * ((LayerStorage.TileBaseOffset / 2) - 1f) + Vector3.up * 5f;
			var rot = Quaternion.Inverse(transform.rotation);
			var euler = rot.eulerAngles;
			euler.y += 90f;
			rot.eulerAngles = euler;

			trigger.transform.rotation = rot;

			trigger.isTrigger = true;
			trigger.gameObject.layer = LayerStorage.ignoreRaycast;
			trigger.size = new Vector3(LayerStorage.TileBaseOffset, 10f, 1f);
			trigger.enabled = false;

			NewLookAnim();
		}

		void NewLookAnim() =>
			lookingAnimation = StartCoroutine(LookingAnimation());
		
		internal void StopLookingAnim(PlayerManager pm)
		{
			if (pm.Tagged)
				return;

			trigger.enabled = false;

			if (lookingAnimation != null)
				StopCoroutine(lookingAnimation);

			Close(false, true, 64, pm.ec);
			audMan.PlaySingle(aud_troll);
			int max = Random.Range(1, 5);
			for (int i = 0; i < max; i++)
				pm.itm.RemoveRandomItem();

			StartCoroutine(StealCooldown());
		}

		IEnumerator StealCooldown()
		{
			while (audMan.AnyAudioIsPlaying) yield return null;

			Close(true, true);
			NewLookAnim();

			yield break;
		}

		IEnumerator LookingAnimation()
		{
			var ec = Singleton<BaseGameManager>.Instance.Ec;
			float frame;
			float cooldown;
			while (true)
			{
				cooldown = Random.Range(minWaitTime, maxWaitTime);
				while (cooldown > 0f)
				{
					cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
					yield return null;
				}

				frame = 9f;
				while (true)
				{
					frame -= speed * ec.EnvironmentTimeScale * Time.deltaTime;
					if (frame > 0f)
						SetMainTex(texs[Mathf.FloorToInt(frame)]);
					else break;
					yield return null;
				}
				trigger.enabled = true;
				yield return null;
				for (int i = 0; i < 3; i++)
				{
					frame = 0f;
					while (true)
					{
						frame += speed * ec.EnvironmentTimeScale * Time.deltaTime;
						if (frame < 6f)
							SetMainTex(texs[Mathf.FloorToInt(frame)]);
						else break;
						yield return null;
					}
					frame = 6f;
					cooldown = maxCool;
					while (cooldown > 0f)
					{
						cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
						yield return null;
					}
					while (true)
					{
						frame -= speed * ec.EnvironmentTimeScale * Time.deltaTime;
						if (frame > 0f)
							SetMainTex(texs[Mathf.FloorToInt(frame)]);
						else break;
						yield return null;
					}
				}
				trigger.enabled = false;
				frame = 0f;
				while (true)
				{
					frame += speed * ec.EnvironmentTimeScale * Time.deltaTime;
					if (frame < 10f)
						SetMainTex(texs[Mathf.FloorToInt(frame)]);
					else break;
					yield return null;
				}
				Close(true, false);
			}
		}

		Coroutine lookingAnimation = null;

		BoxCollider trigger;

		static internal Texture2D[] texs;

		const float maxCool = 1.3f, minWaitTime = 10f, maxWaitTime = 25f, speed = 18f;
	
	}

	public class BlackLockerTrigger : MonoBehaviour
	{
		internal BlackLocker locker;
		private void OnTriggerStay(Collider other)
		{
			if (!other.CompareTag("Player")) return;

			var pm = other.GetComponent<PlayerManager>();
			if (pm)
				locker.StopLookingAnim(pm);
		}
	}
}
