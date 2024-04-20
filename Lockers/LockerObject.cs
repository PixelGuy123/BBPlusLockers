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

		public Type typeOfLocker = t;

		public Color defaultColor = default;

		public bool useDefaultColor = true;

		public float minDistance = 25f, maxDistance = 50f;

		public Locker CreateLocker(HideableLocker target)
		{
			// ****** startup hideable locker ******
			var obj = target.gameObject;
			DestroyImmediate(target); // NO ONE NEEDS YOU!!
			int max = obj.transform.childCount;
			for (int i = 0; i < max; i++)
				Destroy(obj.transform.GetChild(i).gameObject);


			// ** actually implement locker **

			obj.SetActive(false);

			var t = obj.AddComponent(typeOfLocker) as Locker;
			t.closedTex = closedTex;
			t.openTex = openTex;
			t.itemAmountToSteal = itemAmountToSteal;
			t.aud_openLocker = aud_openLocker;
			t.aud_troll = aud_troll;
			t.lockerColor = useDefaultColor ? defaultColor : obj.GetComponent<MeshRenderer>().materials[0].GetColor(LockerCreator.textureColorProperty);
			t.name = t.name;

			obj.SetActive(true); // Make Unity call Awake() here lol

			return t;
		}
	}
}
