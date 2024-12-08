using PixelInternalAPI.Classes;
using UnityEngine;

namespace BBPlusLockers.Lockers.DecoyLockers
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
}
