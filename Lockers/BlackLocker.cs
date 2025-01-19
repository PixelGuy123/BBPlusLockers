using BBPlusLockers.Components;
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

			looker = new(transform, ec)
			{
				visibilityBuffer = -0.25f
			};
		}

		public override void AfterGenCall()
		{
			base.AfterGenCall();
			NewLookAnim();
		}
		
		

		void NewLookAnim()
		{
			if (lookingAnimation != null)
				StopCoroutine(lookingAnimation);
			lookingAnimation = StartCoroutine(LookingAnimation());
		}
			
		
		internal void StopLookingAnim(PlayerManager pm)
		{
			if (pm.Tagged)
				return;

			trigger.enabled = false;

			if (lookingAnimation != null)
				StopCoroutine(lookingAnimation);

			if (looker.IsVisible)
			{
				StartCoroutine(GetScared());
				return;
			}

			Close(false, true, 64);
			audMan.PlaySingle(aud_troll);
			int max = Random.Range(1, 2);
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

		IEnumerator GetScared()
		{
			float frame = 0f;
			int max = fadeOutScaredTextures.Length - 1;

			while (frame < max)
			{
				frame += ec.EnvironmentTimeScale * Time.deltaTime * speed;
				if (frame >= max)
					frame = max;

				SetMainTex(fadeOutScaredTextures[Mathf.FloorToInt(frame)]);
				yield return null;
			}

			Close(true, false);
			NewLookAnim();

			yield break;
		}

		IEnumerator LookingAnimation()
		{
			while (true)
			{
				float cooldown = Random.Range(minWaitTime, maxWaitTime);
				while (cooldown > 0f)
				{
					cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
					yield return null;
				}

				float frame = 0f;
				int max = fadeInTextures.Length - 1;

				while (frame < max)
				{
					frame += ec.EnvironmentTimeScale * Time.deltaTime * speed;
					if (frame >= max)
						frame = max;

					SetMainTex(fadeInTextures[Mathf.FloorToInt(frame)]);
					yield return null;
				}

				trigger.enabled = true;

				cooldown = Random.Range(minLookTime, maxLookTime);
				float lookDelay = delayPerLook;
				bool lookingRight = false;
				frame = 0f;
				max = lookingTextures.Length - 1;

				while (cooldown > 0f || lookingRight)
				{
					cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
					if (lookDelay > 0f)
						lookDelay -= ec.EnvironmentTimeScale * Time.deltaTime;
					else
					{
						if (!lookingRight)
						{
							frame += ec.EnvironmentTimeScale * Time.deltaTime * speed;
							if (frame >= max)
							{
								frame = max;
								lookingRight = true;
								lookDelay = delayPerLook;
							}
						}
						else
						{
							frame -= ec.EnvironmentTimeScale * Time.deltaTime * speed;
							if (frame <= 0)
							{
								frame = 0;
								lookingRight = false;
								lookDelay = delayPerLook;
							}
						}
					}


					SetMainTex(lookingTextures[Mathf.FloorToInt(frame)]);

					yield return null;
				}

				frame = fadeInTextures.Length - 1;
				while (frame > 0)
				{
					frame -= ec.EnvironmentTimeScale * Time.deltaTime * speed;
					if (frame < 0)
						frame = 0;

					SetMainTex(fadeInTextures[Mathf.FloorToInt(frame)]);
					yield return null;
				}

				Close(true, false);

				trigger.enabled = false;

				yield return null;
			}
		}

		Coroutine lookingAnimation;

		BasicLookerInstance looker;

		BoxCollider trigger;

		static internal Texture2D[] lookingTextures, fadeInTextures, fadeOutScaredTextures;

		const float minWaitTime = 10f, maxWaitTime = 15f, speed = 18f,
			minLookTime = 5f, maxLookTime = 12f, delayPerLook = 1f;
	
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
