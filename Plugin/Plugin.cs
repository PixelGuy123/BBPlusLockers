using System.Collections;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;

namespace BBPlusLockers.Plugin
{
	[BepInPlugin(GUIDs.EXTRALOCKERS, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("mtm101.rulerp.bbplus.baldidevapi", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("pixelguy.pixelmodding.baldiplus.pixelinternalapi", BepInDependency.DependencyFlags.HardDependency)]

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
		}

		IEnumerator CreateAssets()
		{
			yield return 2;
			yield return "Creating lockpick...";
			CreateLockpick();
			yield return "Creating locker assets...";
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

				if (modified)
					ld.MarkAsModifiedByMod(Info);
			}
		}

		public static string ModPath = string.Empty;
	}
}
