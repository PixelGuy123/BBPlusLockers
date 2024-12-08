using BBPlusLockers.Lockers.DecoyLockers;
using System;
using UnityEngine;
using static UnityEngine.Object;

namespace BBPlusLockers.Lockers
{
	public class LockerObject(Type t) // data stored here
	{
		public Texture2D closedTex;

		public Texture2D openTex;

		public SoundObject aud_openLocker;

		public int itemAmountToSteal = 1;

		public SoundObject aud_troll;

		protected Type typeOfLocker = t;

		public Color defaultColor = default;

		public bool useDefaultColor = true;

		public float minDistance = 25f, maxDistance = 50f, decoyLaughCooldown = -1f;

		public Locker CreateLocker(GameObject target)
		{
			// ****** startup hideable locker ******
			int max = target.transform.childCount;
			for (int i = 0; i < max; i++)
				Destroy(target.transform.GetChild(i).gameObject);


			// ** actually implement locker **

			target.SetActive(false);

			var t = target.AddComponent(typeOfLocker) as Locker;
			t.closedTex = closedTex;
			t.openTex = openTex;
			t.aud_openLocker = aud_openLocker;
			t.aud_troll = aud_troll; // Aud troll for normal lockers because black locker exists
			if (t is DecoyLocker decLoc)
			{
				decLoc.itemAmountToSteal = itemAmountToSteal;
				decLoc.laughCooldown = decoyLaughCooldown;
			}
			var mat = target.GetComponent<MeshRenderer>();
			t.lockerColor = useDefaultColor ? defaultColor : mat.materials[Locker.colorMatIndex].GetColor(LockerCreator.textureColorProperty);
			if (useDefaultColor)
				mat.materials[Locker.colorMatIndex].mainTexture = baseLockerWhite;
			t.name = t.name;

			target.SetActive(true); // Make Unity call Awake() here lol

			return t;
		}

		internal static Texture2D baseLockerWhite;
	}
}
