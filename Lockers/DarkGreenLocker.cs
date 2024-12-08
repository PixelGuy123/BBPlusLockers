using PixelInternalAPI.Classes;
using PixelInternalAPI.Extensions;
using System.Collections;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class DarkGreenLocker : Locker, IItemAcceptor
	{
		protected override void AwakeFunc()
		{
			base.AwakeFunc();
			var theBlocker = ObjectCreationExtensions.CreateSpriteBillboard(sprite).AddSpriteHolder(out var rend, 0f, 0); // 0 is default
			theBlocker.name = "Blocker";
			rend.name = "BlockerVisual";

			var collider = theBlocker.gameObject.AddComponent<BoxCollider>();
			collider.size = Vector3.one * (LayerStorage.TileBaseOffset - 3.5f);

			var raycastBlocker = Instantiate(collider, collider.transform);
			Destroy(raycastBlocker.transform.GetChild(0).gameObject); // The spritebillboard
			raycastBlocker.transform.localPosition = Vector3.zero;
			raycastBlocker.gameObject.layer = LayerStorage.blockRaycast;

			blocker = collider.transform;

			blocker.SetParent(transform);
			pos = IntVector2.GetGridPosition(transform.position);
			supposedPos = Singleton<BaseGameManager>.Instance.Ec.CellFromPosition(pos).FloorWorldPosition + Vector3.up * 3f;
			blocker.position = supposedPos;

			theBlocker.transform.SetParent(blocker);
			theBlocker.transform.localPosition = Vector3.zero;

			blocker.gameObject.SetActive(false);
		}

		void BlockTiles(bool block, EnvironmentController ec) =>
				Directions.All().ForEach(dir => ec.CellFromPosition(pos + dir.ToIntVector2())?.Block(dir.GetOpposite(), block));
		

		public void InsertItem(PlayerManager pm, EnvironmentController ec)
		{
			openable = false;
			pm.RuleBreak("Lockers", 1.2f, 0.5f);
			blocker.gameObject.SetActive(true);
			Close(false, true, 35);
			BlockTiles(true, ec);

			StartCoroutine(SpawnTheBlocker(pm, ec));
		}

		IEnumerator SpawnTheBlocker(PlayerManager pm, EnvironmentController ec)
		{
			float scale = 0f;
			Vector3 pos = transform.position + Vector3.up * 3f;
			Vector3 ogPos = pos;
			Cell blockCell = ec.CellFromPosition(supposedPos);
			

			while (true)
			{
				scale += (1f - scale) * speed * ec.EnvironmentTimeScale * Time.deltaTime;
				pos = Vector3.Lerp(ogPos, supposedPos, scale);

				if (scale >= 0.98f)
					break;

				if (ec.CellFromPosition(pm.transform.position) == blockCell)
					pm.plm.Entity.AddForce(new(pm.transform.position - supposedPos, 5f, -5f));
				
				blocker.position = pos;
				blocker.localScale = Vector3.one * scale;

				yield return null;
			}

			blocker.position = supposedPos;
			blocker.localScale = Vector3.one;
			scale = 1f;
			pos = supposedPos;
			ogPos = pos;

			Close(true, true);

			float cooldown = 30f;
			while (cooldown > 0f)
			{
				cooldown -= ec.EnvironmentTimeScale * Time.deltaTime;
				yield return null;
			}

			Close(false, true);
			BlockTiles(false, ec);

			while (true)
			{
				scale += -scale * speed * ec.EnvironmentTimeScale * Time.deltaTime;
				pos = Vector3.Lerp(transform.position + Vector3.up * 3f, ogPos, scale);

				if (scale <= 0.08f)
					break;

				blocker.position = pos;
				blocker.localScale = Vector3.one * scale;

				yield return null;
			}

			Close(true, true);

			blocker.gameObject.SetActive(false);
			openable = true;

			yield break;
		}

		public bool ItemFits(Items i) =>
			openable && LockerCreator.CanOpenLocker(i);

		bool openable = true;

		internal static Sprite sprite;

		const float speed = 2.1f;

		Transform blocker;

		Vector3 supposedPos;

		IntVector2 pos;

	}
}
