using UnityEngine;
using MTM101BaldAPI.Registers;
using System.Collections;

namespace BBPlusLockers.Lockers
{
	public class GreenLocker : Locker, IItemAcceptor
	{

		public void InsertItem(PlayerManager player, EnvironmentController ec)
		{
			used = true;
			Close(false, true, 78, ec);
			player.RuleBreak("Lockers", 1.5f, 0.8f);
			StartCoroutine(WaitAndAdd(player));
		}
		public bool ItemFits(Items item) =>
			!used && LockerCreator.CanOpenLocker(item);
		

		IEnumerator WaitAndAdd(PlayerManager pm)
		{
			yield return null;
			pm.itm.AddItem(items[Random.Range(0, items.Length)]);
			yield break;
		}

		bool used = false;


		static ItemObject[] items = [];
		internal static void InitializeItemSelection() =>
			items = ItemMetaStorage.Instance.FindAll(x => !LockerCreator.CanOpenLocker(x.id)).ToValues();
		
			
	}

}
