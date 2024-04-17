using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class YellowLocker : Locker, IClickable<int>
	{
		protected override void AwakeFunc()
		{
			base.AwakeFunc();
			var itmDisplay = Instantiate(LockerCreator.man.Get<SpriteRenderer>("SpriteNoBillboardTemplate"));
			itmDisplay.transform.SetParent(transform);
			itmDisplay.transform.localPosition = -transform.forward * 1.01f + Vector3.up * 3f;
			itmDisplay.transform.rotation = Quaternion.Inverse(transform.rotation);

			Singleton<BaseGameManager>.Instance.Ec.CellFromPosition(transform.position).AddRenderer(itmDisplay);

			itemDisplay = itmDisplay;
		}
		public void Clicked(int player)
		{
			var pm = Singleton<CoreGameManager>.Instance.GetPlayer(player);
			var itmobj = pm.itm.items[pm.itm.selectedItem];
			bool flag = false;
			if (item != null)
			{
				pm.itm.SetItem(item, pm.itm.selectedItem);
				item = null;
				flag = true;
			}
			if (itmobj.itemType != Items.None)
			{
				item = itmobj;
				if (!flag)
					pm.itm.RemoveItem(pm.itm.selectedItem);
				flag = true;
				itemDisplay.sprite = itmobj.itemSpriteLarge; // Expecting every item to be a 64x64
			}
			if (flag)
				Close(true, true, 64, pm.ec);

			itemDisplay.gameObject.SetActive(item != null);

			pm.RuleBreak("Lockers", 0.6f, 1.1f);
		}
		public void ClickableSighted(int player) { }
		public bool ClickableHidden() => false;
		public bool ClickableRequiresNormalHeight() => true;
		public void ClickableUnsighted(int player) { }

		ItemObject item = null;

		SpriteRenderer itemDisplay;
	}
}
