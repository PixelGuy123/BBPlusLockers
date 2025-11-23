using System.Collections;
using System.IO;
using BBPlusLockers.Compats;
using BBPlusLockers.Structures;
using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using PixelInternalAPI;

namespace BBPlusLockers.Plugin
{
	[BepInPlugin(GUIDs.EXTRALOCKERS, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("pixelguy.pixelmodding.baldiplus.pixelinternalapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency(GUIDs.LOADER, BepInDependency.DependencyFlags.SoftDependency)]
	[BepInDependency(GUIDs.STUDIO, BepInDependency.DependencyFlags.SoftDependency)]
	public partial class ExtraLockersPlugin : BaseUnityPlugin
	{
		public static AssetManager assetMan = new();
		private void Awake()
		{
			Harmony h = new(GUIDs.EXTRALOCKERS);
			h.PatchAll();

			ModPath = AssetLoader.GetModPath(this);

			AssetLoader.LoadLocalizationFolder(Path.Combine(ModPath, "Language", "English"), Language.English);

			LoadingEvents.RegisterOnAssetsLoaded(Info, CreateAssets(), LoadingEventOrder.Pre);
			// LoadingEvents.RegisterOnAssetsLoaded(Info, LockerCreator.InitializeAssets(), LoadingEventOrder.Pre);
			// LoadingEvents.RegisterOnAssetsLoaded(Info, GreenLocker.InitializeItemSelection, LoadingEventOrder.Post); // After all the items are added from any mod

			GeneratorManagement.Register(this, GenerationModType.Addend, RegisterGenerationChanges);

			// Very very important for clearing out after a new generation iteration happens
			ResourceManager.AddGenStartCallback((_, _2, _3, _4) => Structure_CustomLockers.replaceableLockers.Clear());

			if (Chainloader.PluginInfos.ContainsKey(GUIDs.LOADER) && Chainloader.PluginInfos.ContainsKey(GUIDs.STUDIO))
				EditorIntegration.Initialize(assetMan);
		}

		IEnumerator CreateAssets()
		{
			yield return 2;
			yield return "Creating lockpick...";
			CreateLockpick();
			yield return "Creating locker assets...";
			GetLockerAssets();
		}

		void RegisterGenerationChanges(string lvlname, int lvlNum, SceneObject sco)
		{
			// Scene Object Changes
			if (shopLockpickWeightsByLevel.TryGetValue(lvlname, out int shopWeight))
				sco.shopItems = sco.shopItems.AddToArray(new() { selection = lockpick, weight = shopWeight });

			// LevelObject changes
			bool modified;
			foreach (var ld in sco.GetCustomLevelObjects())
			{
				if (ld.IsModifiedByMod(Info)) continue;
				// Lockpick setup
				modified = AddLockpickToLevelObject(ld, lvlname);

				// Custom Locker structure setup
				modified = AddCustomLockerToLevelObject(ld, lvlname);

				if (modified)
					ld.MarkAsModifiedByMod(Info);
			}
		}

		public static string ModPath = string.Empty;
	}
}
