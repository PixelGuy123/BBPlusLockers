using PixelInternalAPI.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBPlusLockers.Lockers.DecoyLockers
{
	public class DecoyDarkBlueLocker : AcceptorDecoyLocker
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

			collider.GetComponent<DecoyDarkBlueLockerTrigger>().locker = this;
			collider.gameObject.layer = LayerStorage.ignoreRaycast;
		}
		protected override void ScammedPlayer(PlayerManager pm)
		{
			base.ScammedPlayer(pm);
			force = 0f;
			cooldown = 35f;
			Close(false, true, 50);
			pm.plm.Entity.AddForce(new((pm.transform.position - transform.position).normalized, 6f, -4f));
			StartCoroutine(Vacuum(ec));
		}

		float cooldown = 0f, force = 0f;

		const float maxForce = 35f;

		IEnumerator Vacuum(EnvironmentController ec)
		{
			KeepTrollOpen = true;

			audMan.QueueAudio(DarkBlueLocker.aud_vacuumStart, true);
			audMan.QueueAudio(DarkBlueLocker.aud_vacuumLoop);
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

			KeepTrollOpen = false;

			yield break;
		}

		void UpdateForce()
		{
			for (int i = 0; i < moveMods.Count; i++)
				moveMods[i].movementAddend = (actMods[i].transform.position - transform.position).normalized * force;
		}


		readonly internal List<ActivityModifier> actMods = [];
		readonly internal List<MovementModifier> moveMods = [];
		internal SphereCollider collider;
	}

	public class DecoyDarkBlueLockerTrigger : MonoBehaviour
	{
		internal DecoyDarkBlueLocker locker;
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
