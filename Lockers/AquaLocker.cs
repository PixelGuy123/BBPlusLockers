using System.Collections;
using UnityEngine;
using MTM101BaldAPI.Components;
using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI;

namespace BBPlusLockers.Lockers
{
	public class AquaLocker : Locker, IItemAcceptor
	{

		public void InsertItem(PlayerManager player, EnvironmentController ec)
		{
			used = true;
			Close(false, true, 66);
			player.RuleBreak("Lockers", 1.5f, 0.8f);
			gauge = Singleton<CoreGameManager>.Instance.GetHud(player.playerNumber).gaugeManager.ActivateNewGauge(gaugeSprite, speedTime);
			StartCoroutine(GiveMeSpeed(player, ec));
		}
		public bool ItemFits(Items item) =>
			!used && LockerCreator.CanOpenLocker(item);

		bool used = false;

		IEnumerator GiveMeSpeed(PlayerManager pm, EnvironmentController ec)
		{
			var statModifier = pm.GetMovementStatModifier();
			var modifier = new ValueModifier(2f);
			statModifier.AddModifier("walkSpeed", modifier);
			statModifier.AddModifier("runSpeed", modifier);

			float timer = speedTime;
			while (timer > 0f){
				timer -= ec.EnvironmentTimeScale * Time.deltaTime;
				gauge.SetValue(speedTime, timer);
				yield return null;
			}

			gauge.Deactivate();
			statModifier.RemoveModifier(modifier);
		}
		internal float speedTime = 10f;

		internal static Sprite gaugeSprite;

		protected HudGauge gauge;			
	}

}
