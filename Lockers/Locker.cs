﻿using PixelInternalAPI.Extensions;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public abstract class Locker : MonoBehaviour
	{
		private void Awake()
		{
			Destroy(GetComponent<AudioManager>()); // No normal audio manager
			Destroy(GetComponent<AudioSource>());

			audMan = gameObject.CreatePropagatedAudioManager(minDistance, maxDistance);
			renderer = GetComponent<MeshRenderer>();
			Close(true, false);
			ec = Singleton<BaseGameManager>.Instance.Ec;
			name = GetType().Name;
			SetColor(lockerColor);
			AwakeFunc();
		}

		protected void Close(bool close, bool playNoise)
		{
			Closed = close;
			SetMainTex(close ? closedTex : openTex);
			if (playNoise)
				audMan.PlaySingle(aud_openLocker);
		}

		protected void Close(bool close, bool playNoise, int noiseVal)
		{
			Close(close, playNoise);
			if (noiseVal > 0)
				MakeNoise(noiseVal);
		}

		protected void MakeNoise(int noiseVal) =>
			ec.MakeNoise(ec.CellFromPosition(transform.position).FloorWorldPosition, Mathf.Max(1, noiseVal));
		
		protected void SetColor(Color color) => renderer.materials[colorMatIndex].SetColor(LockerCreator.textureColorProperty, color);
		protected void SetMainTex(Texture tex) => renderer.materials[0].mainTexture = tex;
		protected virtual void AwakeFunc() {}
		public virtual void AfterGenCall() {}

		public Texture2D closedTex;

		public Texture2D openTex;

		public SoundObject aud_openLocker, aud_troll;

		protected AudioManager audMan;

		protected MeshRenderer renderer;

		public Color lockerColor = default;

		public float minDistance = 25f, maxDistance = 50f;

		// protected stuff
		protected bool Closed = true;

		protected EnvironmentController ec;

		internal const int colorMatIndex = 1;
	}
}
