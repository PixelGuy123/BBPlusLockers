using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class YellowLocker : Locker, IClickable<int>
	{
		protected override void AwakeFunc()
		{
			base.AwakeFunc();
			var itmDisplay = ObjectCreationExtensions.CreateSpriteBillboard(null, false);
			itmDisplay.name = "YellowLockerDisplay";
			itmDisplay.transform.SetParent(transform);
			itmDisplay.transform.localPosition = -transform.forward * 1.01f + Vector3.up * 3f;
			itmDisplay.transform.rotation = Quaternion.Inverse(transform.rotation);

			Singleton<BaseGameManager>.Instance.Ec.CellFromPosition(transform.position).AddRenderer(itmDisplay);

			itemDisplay = itmDisplay;
		}
		public void Clicked(int player)
		{
			var pm = Singleton<CoreGameManager>.Instance.GetPlayer(player);
			if (!pm.itm.IsSlotLocked(pm.itm.selectedItem))
			{
				ItemObject itm = pm.itm.items[pm.itm.selectedItem];
				StartCoroutine(AddDelay(item, itm, pm));
				foreach (var loc in displays)
				{
					if (loc != this)
						loc.SetItemDisplay(itm);
				}
			}
		}
		public override void AfterGenCall()
		{
			base.AfterGenCall();
			displays = FindObjectsOfType<YellowLocker>();
		}
		public void SetItemDisplay(ItemObject item)
		{
			this.item = item;
			if (this.item != null && this.item.itemType != Items.None)
			{
				itemDisplay.sprite = this.item.itemSpriteLarge;
				itemDisplay.gameObject.SetActive(true);
				return;
			}
			itemDisplay.gameObject.SetActive(false);
		}
		IEnumerator AddDelay(ItemObject itemToAdd, ItemObject itmReceived, PlayerManager pm) // Modified to support StackableItems properly
		{
			pm.itm.RemoveItem(pm.itm.selectedItem);
			yield return null;
			if (itemToAdd != null)
			{
				pm.itm.AddItem(itemToAdd);
				pm.RuleBreak("Lockers", 0.6f, 1.1f);
			}

			item = itmReceived.itemType == Items.None ? null : itmReceived;
			
				
			if ((itemToAdd != null && itemToAdd.itemType != Items.None) || (itmReceived != null && itmReceived.itemType != Items.None))
				Close(true, true, 64, pm.ec);

			SetItemDisplay(item);
			
			yield break;
		}
		public void ClickableSighted(int player) { }
		public bool ClickableHidden() => false;
		public bool ClickableRequiresNormalHeight() => true;
		public void ClickableUnsighted(int player) { }

		ItemObject item = null;

		SpriteRenderer itemDisplay;

		YellowLocker[] displays;
	}
}
