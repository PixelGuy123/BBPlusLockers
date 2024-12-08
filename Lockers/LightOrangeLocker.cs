using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class LightOrangeLocker : Locker, IItemAcceptor
	{

		public void InsertItem(PlayerManager player, EnvironmentController ec)
		{
			used = true;
			Close(false, true, 66);
			player.RuleBreak("Lockers", 1.5f, 0.8f);
			Singleton<CoreGameManager>.Instance.AddPoints(Random.Range(25, 150), player.playerNumber, true);
		}
		public bool ItemFits(Items item) =>
			!used && LockerCreator.CanOpenLocker(item);

		bool used = false;	
			
	}

}
