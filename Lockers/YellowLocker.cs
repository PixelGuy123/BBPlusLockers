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
			
			StartCoroutine(AddDelay(item, pm.itm.items[pm.itm.selectedItem], pm));
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

			if (item != null)
			{
				itemDisplay.sprite = item.itemSpriteLarge;
				itemDisplay.gameObject.SetActive(true);
				yield break;
			}
			itemDisplay.gameObject.SetActive(false);
			
			yield break;
		}
		public void ClickableSighted(int player) { }
		public bool ClickableHidden() => false;
		public bool ClickableRequiresNormalHeight() => true;
		public void ClickableUnsighted(int player) { }

		ItemObject item = null;

		SpriteRenderer itemDisplay;
	}
}
