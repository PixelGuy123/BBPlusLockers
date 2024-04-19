using BBPlusLockers.Lockers;
using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using System.IO;
using UnityEngine;
using MTM101BaldAPI.Reflection;
using PixelInternalAPI;

namespace BBPlusLockers.Plugin
{
    [BepInPlugin("pixelguy.pixelmodding.baldiplus.bbpluslockers", PluginInfo.PLUGIN_NAME, "1.0.2")]
	[BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("pixelguy.pixelmodding.baldiplus.pixelinternalapi", BepInDependency.DependencyFlags.HardDependency)]

	public class BasePlugin : BaseUnityPlugin
    {
        private void Awake()
        {
        	Harmony h = new("pixelguy.pixelmodding.baldiplus.bbpluslockers");
			h.PatchAll();

			ModPath = AssetLoader.GetModPath(this);

			LoadingEvents.RegisterOnAssetsLoaded(() =>
			{
				try
				{
					var itemobj = new GameObject("Lockpick").AddComponent<ITM_Acceptable>();

					DontDestroyOnLoad(itemobj.gameObject);

					var item = ObjectCreators.CreateItemObject("LPC_Name", "LPC_Desc", 
						AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(ModPath, "lockpick_small.png")), 1f), 
						AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(ModPath, "lockpick.png")), 50f), 
						EnumExtensions.ExtendEnum<Items>("Lockpick"), 45, 20).AddMeta(this, ItemFlags.None).value;

					item.name = "Lockpick";
					item.item = itemobj;
					itemobj.ReflectionSetVariable("item", item.itemType);
					
					lockpick = item;

					ResourceManager.AddWeightedItemToCrazyMachine(new() { selection = lockpick, weight = 55 });

					LockerCreator.InitializeAssets();
				}
				catch (System.Exception e)
				{
					Debug.LogException(e);
					MTM101BaldiDevAPI.CauseCrash(Info, e);
				}
			}, false);

			LoadingEvents.RegisterOnAssetsLoaded(GreenLocker.InitializeItemSelection, true); // After all the items are added from any mod

			GeneratorManagement.Register(this, GenerationModType.Addend, (x, y, z) =>
			{
				z.MarkAsNeverUnload(); // always
				if (x == "F1")
				{
					z.items = z.items.AddToArray(new() { selection = lockpick, weight = 45 });
					z.shopItems = z.shopItems.AddToArray(new() { selection = lockpick, weight = 15});
					return;
				}
				if (x == "F2")
				{
					z.items = z.items.AddToArray(new() { selection = lockpick, weight = 65 }); 
					z.shopItems = z.shopItems.AddToArray(new() { selection = lockpick, weight = 35 });
					z.fieldTripItems.Add(new() { selection = lockpick, weight = 5 });
					return;
				}
				if (x == "F3")
				{
					z.items = z.items.AddToArray(new() { selection = lockpick, weight = 85 });
					z.shopItems = z.shopItems.AddToArray(new() { selection = lockpick, weight = 25 });
					return;
				}
				if (x == "END")
					z.items = z.items.AddToArray(new() { selection = lockpick, weight = 75 });
				
			});
        }

		public static string ModPath = string.Empty;

		internal static ItemObject lockpick; // Will be useful for custom lockers to check for the lockpick;

	}
}
