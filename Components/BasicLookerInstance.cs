using PixelInternalAPI.Classes;
using UnityEngine;

namespace BBPlusLockers.Components
{
	public class BasicLookerInstance(Transform origin, EnvironmentController ec)
	{
		public BasicLookerInstance(Transform origin, EnvironmentController ec, LayerMask mask) : this(origin, ec) =>
			_mask = mask;


		public bool Raycast(Transform target, float rayDistance)
		{
			var offset = target.position - origin.position;
			if (offset.magnitude > rayDistance || _mask != (_mask | (1 << target.gameObject.layer)))
				return false;

			ray.origin = origin.position;
			ray.direction = offset;

			if (Physics.Raycast(ray, out hit, rayDistance, _mask, QueryTriggerInteraction.Ignore))
				return hit.transform == target;
			return false;
		}

		public bool IsVisible
		{
			get
			{
				for (int i = 0; i < Singleton<CoreGameManager>.Instance.setPlayers; i++)
				{
					_angle = Vector3.Angle(-(ec.Players[i].transform.position.ZeroOutY() - origin.position.ZeroOutY()), Singleton<CoreGameManager>.Instance.GetCamera(i).transform.forward);

					float num = Singleton<PlayerFileManager>.Instance.resolutionX / Singleton<PlayerFileManager>.Instance.resolutionY;
					float num2 = Camera.VerticalToHorizontalFieldOfView(Singleton<GlobalCam>.Instance.Cam.fieldOfView, num);

					if (_angle <= num2 / 2f + visibilityBuffer)
						return true;
				}
				return false;
			}
		}


		readonly EnvironmentController ec = ec;
		float _angle;
		public float visibilityBuffer = 7.5f;

		readonly Transform origin = origin;
		readonly LayerMask _mask = LayerStorage.principalLookerMask;

		Ray ray = new();

		RaycastHit hit;
	}
}
