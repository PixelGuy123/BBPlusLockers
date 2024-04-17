using PixelInternalAPI.Extensions;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public abstract class Locker : MonoBehaviour
	{
		private void Awake()
		{
			Destroy(GetComponent<AudioManager>()); // No normal audio manager
			Destroy(GetComponent<AudioSource>());

			audMan = gameObject.CreateAudioManager(minDistance, maxDistance);
			renderer = GetComponent<MeshRenderer>();
			Close(true, false);
			name = GetType().Name;
			SetColor(lockerColor);
			AwakeFunc();
		}

		protected void Close(bool close, bool playNoise)
		{
			Closed = close;
			renderer.materials[1].mainTexture = close ? closedTex : openTex;
			if (playNoise)
				audMan.PlaySingle(aud_openLocker);
		}

		protected void Close(bool close, bool playNoise, int noiseVal, EnvironmentController ec)
		{
			Close(close, playNoise);
			if (noiseVal > 0)
				ec.MakeNoise(transform.position, noiseVal);
		}

		protected void SetColor(Color color) => renderer.materials[0].SetColor(LockerCreator.textureColorProperty, color);
		protected void SetMainTex(Texture tex) => renderer.materials[1].mainTexture = tex;

		protected virtual void AwakeFunc() {}

		public Texture2D closedTex;

		public Texture2D openTex;

		public SoundObject aud_openLocker;

		protected AudioManager audMan;

		protected MeshRenderer renderer;

		public int itemAmountToSteal = 1;

		public SoundObject aud_troll; // specific for decoy lockers

		public Color lockerColor = default;

		public float minDistance = 25f, maxDistance = 50f;

		// protected stuff
		protected bool Closed = true;
	}
}
