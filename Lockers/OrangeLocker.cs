using System.Collections;
using System.Collections.Generic;
using PixelInternalAPI.Classes;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class OrangeLocker : Locker, IClickable<int>
	{
		protected override void AwakeFunc()
		{
			base.AwakeFunc();
			var collider = new GameObject("OrangeLockerCollider").AddComponent<SphereCollider>();
			collider.radius = 35f;
			collider.isTrigger = true;

			collider.transform.SetParent(transform);
			collider.transform.localPosition = -transform.forward * 2f; // Invert forward
			collider.gameObject.layer = LayerStorage.ignoreRaycast;

			trigger = collider.gameObject.AddComponent<OrangeLockerTrigger>();
		}
		public void Clicked(int player)
		{
			if (cooldown > 0) return;

			var pm = Singleton<CoreGameManager>.Instance.GetPlayer(player);

			cooldown = 30f;
			pm.RuleBreak("Lockers", 1.2f, 0.5f);
			Close(false, true, 72);
			StartCoroutine(PushAndWait(ec));
		}
		public void ClickableSighted(int player) { }
		public bool ClickableHidden() => cooldown > 0;
		public bool ClickableRequiresNormalHeight() => true;
		public void ClickableUnsighted(int player) { }

		float cooldown = 0f;

		IEnumerator PushAndWait(EnvironmentController ec)
		{
			float tempCooldown = 5f;
			while (tempCooldown > 0f)
			{
				tempCooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
				yield return null;
			}

			float speed = 0f;
			float frame = 0f;
			int idx;
			while (true)
			{
				speed += 0.2f * ec.EnvironmentTimeScale * Time.deltaTime;
				frame += speed;
				idx = Mathf.FloorToInt(frame);
				if (idx < openTexs.Length)
					SetMainTex(openTexs[idx]);
				else break;
				yield return null;
			}
			Close(true, true);

			for (int i = 0; i < trigger.entities.Count; i++)
				trigger.entities[i].AddForce(new((trigger.entities[i].transform.position - transform.position).normalized, 75f, -25f));


			while (cooldown > 0f)
			{
				cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
				yield return null;
			}

			yield break;
		}

		internal static Texture2D[] openTexs;

		OrangeLockerTrigger trigger;
	}

	public class OrangeLockerTrigger : MonoBehaviour
	{
		internal List<Entity> entities = [];
		private void OnTriggerEnter(Collider other)
		{
			var entity = other.GetComponent<Entity>();
			if (entity)
				entities.Add(entity);
		}

		private void OnTriggerExit(Collider other)
		{
			var entity = other.GetComponent<Entity>();
			if (entity)
				entities.Remove(entity);
		}
	}
}
