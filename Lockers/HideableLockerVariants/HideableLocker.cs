using BBPlusLockers.Plugin;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BBPlusLockers.Lockers.HideableLockerVariants
{
    public abstract class HideableLocker : Locker, IClickable<int> // Basically a copypaste of HideableLocker, but supports the Locker type
    {
        protected override void AwakeFunc()
        {
            base.AwakeFunc();
            cameraTransform = new GameObject("HideableLocker_Cam").transform;
            cameraTransform.SetParent(transform);
            cameraTransform.localPosition = Vector3.up * 5f;
			cameraTransform.localRotation = Quaternion.Euler(0, 180f, 0);

            hud = Instantiate(hideableLockerCanvasPre);
            hud.name = "HideableLockerCanvas";
            hud.transform.SetParent(transform);
            hud.gameObject.SetActive(false);

            image = hud.GetComponentInChildren<Image>();
			image.sprite = HudImage();
		}
		protected abstract Sprite HudImage();
		protected virtual Texture2D TextureWhenOnSight() => null;
		protected abstract void LockerUsed();
		protected virtual bool CanLockerBeUsed() => true;
		protected virtual bool CanMakeNoise() => true;
        public void Clicked(int player)
        {
			if (!CanLockerBeUsed())
				return;

            Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.Entity.SetInteractionState(false);
            Singleton<CoreGameManager>.Instance.GetPlayer(player).plm.Entity.SetFrozen(true);
            Singleton<CoreGameManager>.Instance.GetCamera(player).SetControllable(false);
            Singleton<CoreGameManager>.Instance.GetCamera(player).UpdateTargets(cameraTransform, 20);
            playerInLocker = Singleton<CoreGameManager>.Instance.GetPlayer(player);

            StartCoroutine(CameraReset(playerInLocker));

			if (CanMakeNoise())
				MakeNoise(noiseVal);
            audMan.PlaySingle(audSlam);

            hud.gameObject.SetActive(true);
            hud.worldCamera = Singleton<CoreGameManager>.Instance.GetCamera(player).canvasCam;
        }

        public void ClickableSighted(int player)
		{
			if (BasePlugin.hasAnimations && CanLockerBeUsed() && TextureWhenOnSight() != null)
				SetMainTex(TextureWhenOnSight());
		}
		public void ClickableUnsighted(int player) 
		{
			if (BasePlugin.hasAnimations && CanLockerBeUsed())
				Close(true, false);
		}
		public bool ClickableHidden() => !CanLockerBeUsed();
        public bool ClickableRequiresNormalHeight() => true;
       

        private IEnumerator CameraReset(PlayerManager player)
        {
            Vector3 pos = player.transform.position;
            yield return null;
            while (player == playerInLocker)
            {
				if (CanMakeNoise())
					MakeNoise(noiseVal);

                if (!Singleton<CoreGameManager>.Instance.Paused && (Singleton<InputManager>.Instance.GetDigitalInput("Interact", true) || player.transform.position != pos))
                {
                    player.plm.Entity.SetInteractionState(true);
                    player.plm.Entity.SetFrozen(false);
                    playerInLocker = null;
                }
                if (Singleton<CoreGameManager>.Instance.Paused)
                {
                    hud.gameObject.SetActive(false);
                }
                else
                {
                    hud.gameObject.SetActive(true);
                }
                yield return null;
            }
			LockerUsed();

			if (CanMakeNoise())
				MakeNoise(noiseVal);

            audMan.PlaySingle(audSlam);
            Singleton<CoreGameManager>.Instance.GetCamera(player.playerNumber).UpdateTargets(null, 20);
            Singleton<CoreGameManager>.Instance.GetCamera(player.playerNumber).SetControllable(true);
            hud.gameObject.SetActive(false);
            player.RuleBreak("Lockers", guiltTime, 1.5f);
            yield break;
        }

        PlayerManager playerInLocker;

        protected int noiseVal = 78;

        protected float guiltTime = 2.5f;

        protected Canvas hud;

        protected Image image;

        protected Transform cameraTransform;

        static internal SoundObject audSlam;

		static internal Canvas hideableLockerCanvasPre;
    }
}
