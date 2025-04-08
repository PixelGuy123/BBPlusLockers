using HarmonyLib;
using MTM101BaldAPI.Registers;
using PixelInternalAPI.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class GreenLocker : Locker, IItemAcceptor
	{

		public void InsertItem(PlayerManager player, EnvironmentController ec)
		{
			used = true;
			Close(false, true, 78);
			player.RuleBreak("Lockers", 1.5f, 0.8f);
			StartCoroutine(WaitAndAdd(player));
		}
		public bool ItemFits(Items item) =>
			!used && LockerCreator.CanOpenLocker(item);


		IEnumerator WaitAndAdd(PlayerManager pm)
		{
			yield return null;
			pm.itm.AddItem(items[Random.Range(0, items.Count)]);
			yield break;
		}

		bool used = false;


		static List<ItemObject> items = [];
		internal static void InitializeItemSelection()
		{
			items = [];
			foreach (var s in GenericExtensions.FindResourceObjects<SceneObject>())
			{
				s.shopItems.Do(x =>
				{
					var meta = x.selection.GetMeta();
					if (meta != null && !items.Contains(meta.value))
						items.Add(meta.value);
				});
			}
		}


	}

}
