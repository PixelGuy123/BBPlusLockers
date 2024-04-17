using PixelInternalAPI.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class DecoyOrangeLocker : AcceptorDecoyLocker
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

		protected override void AfterTrollAndClose(PlayerManager pm)
		{
			base.AfterTrollAndClose(pm);
			for (int i = 0; i < trigger.entities.Count; i++)
				trigger.entities[i].AddForce(new((trigger.entities[i].transform.position - transform.position).normalized, 105f, -45f));
			
		}

		OrangeLockerTrigger trigger;
	}

	public class OrangeLocker : Locker, IItemAcceptor
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
		public void InsertItem(PlayerManager pm, EnvironmentController ec)
		{
			cooldown = 30f;
			pm.RuleBreak("Lockers", 1.2f, 0.5f);
			Close(false, true, 72, ec);
			StartCoroutine(PushAndWait(ec));
		}

		public bool ItemFits(Items i) =>
			cooldown <= 0f && LockerCreator.CanOpenLocker(i);

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
