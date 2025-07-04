using System.Collections;
using System.IO;
using BBPlusLockers.Lockers;
using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Registers;
using PixelInternalAPI;
using PixelInternalAPI.Extensions;

namespace BBPlusLockers.Plugin
{
	[BepInPlugin(guid, PluginInfo.PLUGIN_NAME, "1.1.5")]
	[BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("pixelguy.pixelmodding.baldiplus.pixelinternalapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("pixelguy.pixelmodding.baldiplus.newanimations", BepInDependency.DependencyFlags.SoftDependency)]

	public class BasePlugin : BaseUnityPlugin
	{
		internal static bool hasAnimations = false;
		internal const string guid = "pixelguy.pixelmodding.baldiplus.bbpluslockers", customLockersDataKey = "CustomLockers";
		private void Awake()
		{
			Harmony h = new(guid);
			h.PatchAll();

			ModPath = AssetLoader.GetModPath(this);

			AssetLoader.LoadLocalizationFolder(Path.Combine(ModPath, "Language", "English"), Language.English);

			LoadingEvents.RegisterOnAssetsLoaded(Info, CreateLockPick(), LoadingEventOrder.Pre);
			LoadingEvents.RegisterOnAssetsLoaded(Info, LockerCreator.InitializeAssets(), LoadingEventOrder.Pre);

			LoadingEvents.RegisterOnAssetsLoaded(Info, GreenLocker.InitializeItemSelection, LoadingEventOrder.Post); // After all the items are added from any mod

			hasAnimations = Chainloader.PluginInfos.ContainsKey("pixelguy.pixelmodding.baldiplus.newanimations");

			GeneratorManagement.Register(this, GenerationModType.Addend, (x, y, sco) =>
			{
				bool added = false;
				foreach (var z in sco.GetCustomLevelObjects())
				{
					z.MarkAsNeverUnload(); // always

					//UnityEngine.Debug.Log(z.name);

					if (LockerCreator.lockers.TryGetValue(x, out var lockerList))
						z.SetCustomModValue(Info, customLockersDataKey, lockerList);

					if (x == "F1")
					{
						z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 45 });
						if (!added)
						{
							sco.shopItems = sco.shopItems.AddToArray(new() { selection = lockpick, weight = 15 });
							added = true;
						}
						z.forcedItems.Add(lockpick);
						z.forcedItems.Add(lockpick);
						continue;
					}
					if (x == "F2")
					{
						z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 65 });
						if (!added)
						{
							sco.shopItems = sco.shopItems.AddToArray(new() { selection = lockpick, weight = 35 });
							added = true;
						}
						//z.fieldTripItems.Add(new() { selection = lockpick, weight = 5 });
						z.forcedItems.Add(lockpick);
						continue;
					}
					if (x == "F3")
					{
						z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 85 });
						if (!added)
						{
							sco.shopItems = sco.shopItems.AddToArray(new() { selection = lockpick, weight = 25 });
							added = true;
						}
						z.forcedItems.Add(lockpick);
						continue;
					}
					if (x == "F4")
					{
						z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 64 });
						if (!added)
						{
							sco.shopItems = sco.shopItems.AddToArray(new() { selection = lockpick, weight = 45 });
							added = true;
						}
						continue;
					}
					if (x == "F5")
					{
						z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 55 });
						if (!added)
						{
							sco.shopItems = sco.shopItems.AddToArray(new() { selection = lockpick, weight = 35 });
							added = true;
						}
						continue;
					}
					if (x == "END")
						z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 75 });
				}

			});
		}

		IEnumerator CreateLockPick()
		{
			yield return 1;
			yield return "Creating lock pick...";
			var item = new ItemBuilder(Info)
				.SetEnum("Lockpick")
				.SetShopPrice(350)
				.SetGeneratorCost(20)
				.SetItemComponent<ITM_Acceptable>()
				.SetSprites(AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(ModPath, "lockpick_small.png")), 1f),
				AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(ModPath, "lockpick.png")), 50f))
				.SetNameAndDescription("LPC_Name", "LPC_Desc")
				.SetMeta(ItemFlags.None, ["StackableItems_NotAllowStacking"])
				.Build();
			((ITM_Acceptable)item.item).item = item.itemType;
			((ITM_Acceptable)item.item).layerMask = GenericExtensions.FindResourceObjectByName<LayerMaskObject>("PlayerClickLayerMask");

			lockpick = item;
			lockpick.AddKeyTypeItem();

			ResourceManager.AddWeightedItemToCrazyMachine(new() { selection = lockpick, weight = 55 });
			ResourceManager.AddPostGenCallback((_) => FindObjectsOfType<Locker>().Do(x => x.AfterGenCall()));
		}

		public static string ModPath = string.Empty;

		internal static ItemObject lockpick; // Will be useful for custom lockers to check for the lockpick;

	}
}
