using MTM101BaldAPI;
using PixelInternalAPI.Extensions;
using PixelInternalAPI.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public class TurquoiseLocker : Locker, IItemAcceptor
	{
		protected override void AwakeFunc()
		{
			base.AwakeFunc();

			water = ObjectCreationExtensions.CreateSpriteBillboard(sprite, false).AddSpriteHolder(out var watRenderer, 0, LayerStorage.ignoreRaycast).gameObject.AddComponent<TurquoiseLockerWater>();
			water.name = "Water";
			water.transform.SetParent(transform);
			water.transform.localPosition = Vector3.zero;
			watRenderer.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

			var collider = water.gameObject.AddComponent<BoxCollider>();
			collider.isTrigger = true;
			collider.size = Vector3.one * 8.5f;

			water.gameObject.SetActive(false);
		}
		

		public void InsertItem(PlayerManager pm, EnvironmentController ec)
		{
			openable = false;
			pm.RuleBreak("Lockers", 1.2f, 0.5f);
			water.gameObject.SetActive(true);
			Close(false, true, 35);

			StartCoroutine(SpawnTheBlocker(ec));
		}

		IEnumerator SpawnTheBlocker(EnvironmentController ec)
		{
			water.transform.position = ec.CellFromPosition(transform.position).FloorWorldPosition + Vector3.up * 0.05f;
			float t = 0f;
			while (true)
			{
				t += ec.EnvironmentTimeScale * Time.deltaTime * 6f;
				if (t >= 1f)
					break;

				water.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
			}

			water.transform.localScale = Vector3.one;

			yield return new WaitForSecondsEnvironmentTimescale(ec, 15f);

			while (true)
			{
				t += ec.EnvironmentTimeScale * Time.deltaTime * 6f;
				if (t >= 1f)
					break;

				water.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			}

			water.Disable();
			Close(true, true);

			yield break;
		}

		public bool ItemFits(Items i) =>
			openable && LockerCreator.CanOpenLocker(i);

		bool openable = true;

		internal static Sprite sprite;

		TurquoiseLockerWater water;

	}

	public class TurquoiseLockerWater : MonoBehaviour
	{
		public void Disable()
		{
			while (entities.Count != 0)
			{
				entities[0]?.ExternalActivity.moveMods.Remove(moveMod);
				entities.RemoveAt(0);
			}
			gameObject.SetActive(false);
		}
		void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger && (other.CompareTag("NPC") || other.CompareTag("Player")))
			{
				var e = other.GetComponent<Entity>();
				if (e)
				{
					e.ExternalActivity.moveMods.Add(moveMod);
					entities.Add(e);
				}
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.isTrigger && (other.CompareTag("NPC") || other.CompareTag("Player")))
			{
				var e = other.GetComponent<Entity>();
				if (e)
				{
					e.ExternalActivity.moveMods.Remove(moveMod);
					entities.Remove(e);
				}
			}
		}

		readonly List<Entity> entities = [];

		readonly MovementModifier moveMod = new(Vector3.zero, 0.5f);
	}
}
