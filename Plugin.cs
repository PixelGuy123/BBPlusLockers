﻿using BBPlusLockers.Lockers;
using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using System.IO;
using MTM101BaldAPI.Reflection;
using BepInEx.Bootstrap;
using PixelInternalAPI;
using System.Collections;
using MTM101BaldAPI.ObjectCreation;

namespace BBPlusLockers.Plugin
{
    [BepInPlugin("pixelguy.pixelmodding.baldiplus.bbpluslockers", PluginInfo.PLUGIN_NAME, "1.1.3")]
	[BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("pixelguy.pixelmodding.baldiplus.pixelinternalapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("pixelguy.pixelmodding.baldiplus.newanimations", BepInDependency.DependencyFlags.SoftDependency)]

	public class BasePlugin : BaseUnityPlugin
    {
		internal static bool hasAnimations = false;
        private void Awake()
        {
        	Harmony h = new("pixelguy.pixelmodding.baldiplus.bbpluslockers");
			h.PatchAll();

			ModPath = AssetLoader.GetModPath(this);

			AssetLoader.LoadLocalizationFolder(Path.Combine(ModPath, "Language", "English"), Language.English);
			
			LoadingEvents.RegisterOnAssetsLoaded(Info, CreateLockPick(), false);
			LoadingEvents.RegisterOnAssetsLoaded(Info, LockerCreator.InitializeAssets(), false);

			LoadingEvents.RegisterOnAssetsLoaded(Info, GreenLocker.InitializeItemSelection, true); // After all the items are added from any mod

			hasAnimations = Chainloader.PluginInfos.ContainsKey("pixelguy.pixelmodding.baldiplus.newanimations");

			GeneratorManagement.Register(this, GenerationModType.Addend, (x, y, sco) =>
			{
				var z = sco.levelObject;
				if (z == null)
					return;

				z.MarkAsNeverUnload(); // always
				if (x == "F1")
				{
					z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 45 });
					z.shopItems = z.shopItems.AddToArray(new() { selection = lockpick, weight = 15});
					z.forcedItems.Add(lockpick);
					z.forcedItems.Add(lockpick);
					return;
				}
				if (x == "F2")
				{
					z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 65 }); 
					z.shopItems = z.shopItems.AddToArray(new() { selection = lockpick, weight = 35 });
					//z.fieldTripItems.Add(new() { selection = lockpick, weight = 5 });
					z.forcedItems.Add(lockpick);
					return;
				}
				if (x == "F3")
				{
					z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 85 });
					z.shopItems = z.shopItems.AddToArray(new() { selection = lockpick, weight = 25 });
					z.forcedItems.Add(lockpick);
					return;
				}
				if (x == "END")
					z.potentialItems = z.potentialItems.AddToArray(new() { selection = lockpick, weight = 75 });
				
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
				.SetMeta(ItemFlags.None, [])
				.Build();
			((ITM_Acceptable)item.item).ReflectionSetVariable("item", item.itemType);

			lockpick = item;
			lockpick.AddKeyTypeItem();

			ResourceManager.AddWeightedItemToCrazyMachine(new() { selection = lockpick, weight = 55 });
			ResourceManager.AddPostGenCallback((_) => FindObjectsOfType<Locker>().Do(x => x.AfterGenCall()));
		}

		public static string ModPath = string.Empty;

		internal static ItemObject lockpick; // Will be useful for custom lockers to check for the lockpick;

	}
}
