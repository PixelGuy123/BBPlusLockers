using System.Collections;

namespace BBPlusLockers.Lockers
{
	// Base class
	public abstract class DecoyLocker : Locker
	{
		protected void OpenAndScamPlayer(PlayerManager pm)
		{
			for (int i = 0; i < itemAmountToSteal; i++)
				pm.itm.RemoveRandomItem();

			pm.RuleBreak("Lockers", 1.5f, 0.8f);
			Close(false, true, 65);
			audMan.PlaySingle(aud_troll);
			StartCoroutine(Cooldown(pm));
			opened = true;
		}

		protected virtual void AfterTrollAndClose(PlayerManager pm) { }

		IEnumerator Cooldown(PlayerManager pm)
		{
			while (audMan.AnyAudioIsPlaying) yield return null;
			audMan.PlaySingle(aud_openLocker);
			Close(true, true);

			AfterTrollAndClose(pm);

			yield break;
		}

		protected bool opened = false;
	}
	// Item acceptor decoy locker
	public class AcceptorDecoyLocker : DecoyLocker, IItemAcceptor
	{
		public bool ItemFits(Items it) =>
			!opened && LockerCreator.CanOpenLocker(it);
		public void InsertItem(PlayerManager pm, EnvironmentController ec) =>
			OpenAndScamPlayer(pm);
	}

	// Clickable decoylocker
	public class ClickableDecoyLocker : DecoyLocker, IClickable<int>
	{
		public void Clicked(int player)
		{
			if (!opened)
				OpenAndScamPlayer(Singleton<CoreGameManager>.Instance.GetPlayer(player));
		}
		public void ClickableSighted(int player) { }
		public bool ClickableHidden() => opened;
		public bool ClickableRequiresNormalHeight() => true;
		public void ClickableUnsighted(int player) { }
	}
}
