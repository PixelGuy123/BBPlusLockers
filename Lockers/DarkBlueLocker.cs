using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using PixelInternalAPI.Classes;

namespace BBPlusLockers.Lockers
{
	public class DarkBlueLocker : Locker, IItemAcceptor
	{

		protected override void AwakeFunc()
		{
			base.AwakeFunc();
			collider = new GameObject("DarkBlueLockerCollider", typeof(DarkBlueLockerTrigger)).AddComponent<SphereCollider>();
			collider.radius = 19f;
			collider.isTrigger = true;
			collider.enabled = false;

			collider.transform.SetParent(transform);
			collider.transform.localPosition = -transform.forward * 2f; // Invert forward

			collider.GetComponent<DarkBlueLockerTrigger>().locker = this;
			collider.gameObject.layer = LayerStorage.ignoreRaycast;
		}
		public void InsertItem(PlayerManager pm, EnvironmentController ec)
		{
			force = 0f;
			cooldown = 35f;
			Close(false, true, 45);
			pm.plm.Entity.AddForce(new((pm.transform.position - transform.position).normalized, 6f, -4f));
			pm.RuleBreak("Lockers", 1.2f, 0.5f);			
			StartCoroutine(Vacuum(ec));
		}

		public bool ItemFits(Items i) =>
			cooldown <= 0f && LockerCreator.CanOpenLocker(i);

		float cooldown = 0f, force = 0f;

		const float maxForce = 25f;

		internal static SoundObject aud_vacuumStart, aud_vacuumLoop;

		IEnumerator Vacuum(EnvironmentController ec)
		{
			audMan.QueueAudio(aud_vacuumStart, true);
			audMan.QueueAudio(aud_vacuumLoop);
			audMan.SetLoop(true);
			float vacuumCooldown = Random.Range(7f, 15f);
			collider.enabled = true;

			while (vacuumCooldown > 0f)
			{
				force += 2.8f * ec.EnvironmentTimeScale * Time.deltaTime;
				if (force > maxForce)
					force = maxForce;
				vacuumCooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
				UpdateForce();
				yield return null;
			}

			collider.enabled = false;

			audMan.FlushQueue(true);
			while (actMods.Count != 0)
			{
				actMods[0].moveMods.Remove(moveMods[0]);
				actMods.RemoveAt(0);
				moveMods.RemoveAt(0);
			}

			Close(true, true);

			while (cooldown > 0f)
			{
				cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
				yield return null;
			}

			yield break;
		}

		void UpdateForce()
		{
			for (int i = 0; i < moveMods.Count; i++)
				moveMods[i].movementAddend = (transform.position - actMods[i].transform.position).normalized * force;
		}


		readonly internal List<ActivityModifier> actMods = [];
		readonly internal List<MovementModifier> moveMods = [];
		internal SphereCollider collider;
	}

	public class DarkBlueLockerTrigger : MonoBehaviour
	{
		internal DarkBlueLocker locker;
		void OnTriggerEnter(Collider other)
		{
			var entity = other.GetComponent<Entity>();
			if (!entity) return;
			if (!locker.actMods.Contains(entity.ExternalActivity))
			{
				MovementModifier moveMod = new(Vector3.zero, 0.2f, 5);
				entity.ExternalActivity.moveMods.Add(moveMod);
				locker.moveMods.Add(moveMod);
				locker.actMods.Add(entity.ExternalActivity);
			}
		}

		void OnTriggerExit(Collider other)
		{
			var entity = other.GetComponent<Entity>();
			if (!entity) return;
			int idx = locker.actMods.IndexOf(entity.ExternalActivity);
			if (idx != -1)
			{
				entity.ExternalActivity.moveMods.Remove(locker.moveMods[idx]);
				locker.moveMods.RemoveAt(idx);
				locker.actMods.RemoveAt(idx);
			}
		}
	}
}
