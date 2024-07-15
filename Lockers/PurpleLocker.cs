using UnityEngine;
using System.Collections;
using MTM101BaldAPI.Components;
using PixelInternalAPI.Classes;
using PixelInternalAPI.Components;
using PixelInternalAPI.Extensions;

namespace BBPlusLockers.Lockers
{
	public class PurpleLocker : Locker, IItemAcceptor
	{
		protected override void AwakeFunc()
		{
			base.AwakeFunc();

			var renderer = ObjectCreationExtensions.CreateSpriteBillboard(null, false).AddSpriteAnimator(out animator);
			renderer.name = "PurplePortal";
			animator.spriteRenderer = renderer;
			animator.animations.Add("loop", animation);
			animator.SetDefaultAnimation("loop", 0.7f);
			animator.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

			animator.gameObject.SetActive(false);

			trigger = new GameObject("PurpleLockerTrigger", typeof(SphereCollider)).AddComponent<PurpleLockerTrigger>();
			var c = trigger.GetComponent<SphereCollider>();
			c.isTrigger = true;
			c.radius = 2f;

			trigger.parent = this;

			trigger.gameObject.layer = LayerStorage.ignoreRaycast;
			trigger.transform.SetParent(transform);
			trigger.transform.localPosition = -transform.forward * (LayerStorage.TileBaseOffset / 2) + Vector3.up * 5f;
			trigger.gameObject.SetActive(false);
		}

		public void InsertItem(PlayerManager player, EnvironmentController ec)
		{
			active = true;
			Close(false, true, 78, ec);
			trigger.prevTarget = player.plm.Entity;
			trigger.gameObject.SetActive(true);
			StartCoroutine(WaitForWarp(ec));
		}
		public bool ItemFits(Items item) =>
			!active && LockerCreator.CanOpenLocker(item);
		
		IEnumerator SpawnTeleporter(EnvironmentController ec, Vector3 pos, bool despawn)
		{
			if (!despawn)
			{
				animator.gameObject.SetActive(true);
				animator.transform.position = pos;
				float scale = 0f;
				while (scale < 1f)
				{
					scale += Mathf.Clamp01(tpSpeed * 2f * ec.EnvironmentTimeScale * Time.deltaTime);
					animator.transform.localScale = Vector3.one * scale;
					yield return null;
				}
			}
			else
			{
				float cooldown = 3f;
				while (cooldown > 0f)
				{
					cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
					yield return null;
				}	
				float scale = 1f;
				while (scale > 0f)
				{
					scale -= Mathf.Clamp01(tpSpeed * 2f * ec.EnvironmentTimeScale * Time.deltaTime);
					animator.transform.localScale = Vector3.one * scale;
					yield return null;
				}
				animator.gameObject.SetActive(false);
			}

			yield break;
		}

		IEnumerator WaitForWarp(EnvironmentController ec)
		{
			while (foundTarget == null) yield return null;
			trigger.gameObject.SetActive(false);

			var cells = ec.AllTilesNoGarbage(false, false);
			Vector3 selPos = cells[Random.Range(0, cells.Count)].FloorWorldPosition;

			StartCoroutine(SpawnTeleporter(ec, selPos + Vector3.up * 0.05f, false));

			Entity e = foundTarget;
			foundTarget = null;

			e.Teleport(transform.position + (-transform.forward * 4f + Vector3.up * e.Height));
			e.SetFrozen(true);
			e.SetInteractionState(false);
			e.SetTrigger(false);

			var pm = foundTarget.GetComponent<PlayerManager>();

			float t = 0f;
			Vector3 pos = e.transform.position;

			while (t < 1f)
			{
				t += Mathf.Clamp01(tpSpeed * ec.EnvironmentTimeScale * Time.deltaTime);
				e.Teleport(Vector3.Lerp(pos, transform.position, t));
				yield return null;
			}
			audMan.PlaySingle(aud_tp);
			e.Teleport(selPos);
			if (pm)
				pm.GetCustomCam().ReverseSlideFOVAnimation(new ValueModifier(), 35f, 3.5f);

			float height = e.Height;
			e.SetHeight(sinkHeight);

			t = 0f;

			while (t < 1f)
			{
				t += Mathf.Clamp01(sinkSpeed * ec.EnvironmentTimeScale * Time.deltaTime);
				e.SetHeight(Mathf.Lerp(sinkHeight, height, t));
				yield return null;
			}

			e.SetFrozen(false);
			e.SetInteractionState(true);
			e.SetTrigger(true);

			StartCoroutine(SpawnTeleporter(ec, default, true));

			active = false;
			Close(true, true);

			yield break;
		}

		bool active = false;

		internal static CustomAnimation<Sprite> animation;
		internal static SoundObject aud_tp;

		CustomSpriteAnimator animator;
		PurpleLockerTrigger trigger;

		internal Entity foundTarget = null;

		const float tpSpeed = 0.35f, sinkSpeed = 0.25f, sinkHeight = 0.5f;
			
	}

	public class PurpleLockerTrigger : MonoBehaviour
	{
		internal PurpleLocker parent;

		internal Entity prevTarget = null;
		void OnTriggerEnter(Collider other)
		{
			var e = other.GetComponent<Entity>();
			if (e && prevTarget != e)
				parent.foundTarget = e;
		}

		void OnTriggerExit(Collider other)
		{
			var e = other.GetComponent<Entity>();
			if (e && prevTarget == e)
				prevTarget = null;
		}
	}

}
