using BBPlusLockers.Plugin;
using System.Collections.Generic;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections;
using PixelInternalAPI.Extensions;
using BBPlusLockers.Lockers.DecoyLockers;
using BBPlusLockers.Lockers.HideableLockerVariants;

namespace BBPlusLockers.Lockers
{
	public static class LockerCreator
	{

		internal static IEnumerator InitializeAssets() // Mods can *patch* this method with postfix to include their Items that lockers can accept
		{
			yield return enumeratorSize;

			lockers.Add(new() { selection = null, weight = 250 }); // Null means the already default locker

			LockerObject.baseLockerWhite = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "baseLockerSide.png"));
			var trueHideableLocker = GenericExtensions.FindResourceObject<HideableLocker>();
			HideableLockerVariants.HideableLocker.audSlam = trueHideableLocker.audSlam;
			SoundObject defaultAudio = GenericExtensions.FindResourceObjectByName<SoundObject>("Doors_Locker");

			var texs = TextureExtensions.LoadTextureSheet(2, 1, BasePlugin.ModPath, "decoyBlueLocker.png");

			yield return "Creating decoy blue locker...";
			// Decoy Blue Locker
			var locker = new LockerObject(typeof(ClickableDecoyLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "trololo.wav")), "Vfx_Locker_trololo", SoundType.Voice, Color.white),
				useDefaultColor = false,
				itemAmountToSteal = 1
			};

			lockers.Add(new() { selection = locker, weight = 15 });

			yield return "Creating green locker...";

			texs = TextureExtensions.LoadTextureSheet(2, 2, BasePlugin.ModPath, "greenLockers.png");

			
			// Green Locker
			locker = new LockerObject(typeof(GreenLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[3],
				closedTex = texs[2],
				defaultColor = Color.green
			};

			lockers.Add(new() { selection = locker, weight = 25 });

			yield return "Creating decoy green locker...";
			locker = new LockerObject(typeof(AcceptorDecoyLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "HA_HA.wav")), "Vfx_Locker_HAHA", SoundType.Voice, Color.white),
				defaultColor = Color.green,
				itemAmountToSteal = 0
			};

			lockers.Add(new() { selection = locker, weight = 25 });

			texs = TextureExtensions.LoadTextureSheet(2, 2, BasePlugin.ModPath, "darkBlueLocker.png");

			// Dark blue locker (PULL FORCE)
			yield return "Creating dark blue locker...";
			locker = new LockerObject(typeof(DarkBlueLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				defaultColor = new(0.01171875f, 0.01171875f, 0.99609375f) // dark blue
			};
			DarkBlueLocker.aud_vacuumStart = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "vacuum_start.wav")), "Vfx_Locker_vacuum", SoundType.Voice, Color.white);
			DarkBlueLocker.aud_vacuumLoop = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "vacuum_loop.wav")), "Vfx_Locker_vacuum", SoundType.Voice, Color.white);

			lockers.Add(new() { selection = locker, weight = 35 });
			yield return "Creating decoy dark blue locker...";
			locker = new LockerObject(typeof(DecoyDarkBlueLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[3],
				closedTex = texs[2],
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "deepHA_HA.wav")), "Vfx_Locker_deepHAHA", SoundType.Voice, Color.white),
				defaultColor = new(0.01171875f, 0.01171875f, 0.99609375f), // dark blue
				itemAmountToSteal = 0
			};

			lockers.Add(new() { selection = locker, weight = 25 });

			texs = TextureExtensions.LoadTextureSheet(8, 1, BasePlugin.ModPath, "orangeLocker.png");

			// Orange Locker (Push force)
			yield return "Creating orange locker...";
			var orangeLockerAudio = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "deepslam.wav")), "Vfx_Locker_deepSLAM", SoundType.Voice, Color.white);
			locker = new LockerObject(typeof(OrangeLocker))
			{
				aud_openLocker = orangeLockerAudio,
				openTex = texs[3],
				closedTex = texs[2],
				defaultColor = new(0.99609375f, 0.61328125f, 0.04296875f) // orange
			};

			OrangeLocker.openTexs = [.. texs.Skip(4)];


			lockers.Add(new() { selection = locker, weight = 15 });

			yield return "Creating decoy orange locker...";

			locker = new LockerObject(typeof(DecoyOrangeLocker))
			{
				aud_openLocker = orangeLockerAudio,
				openTex = texs[1],
				closedTex = texs[0],
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "heheheha.wav")), "Vfx_Locker_heheha", SoundType.Voice, Color.white),
				defaultColor = new(0.99609375f, 0.61328125f, 0.04296875f), // orange
				itemAmountToSteal = 1
			};


			lockers.Add(new() { selection = locker, weight = 5 });

			// Yellow Locker (store item)
			yield return "Creating yellow locker...";

			locker = new LockerObject(typeof(YellowLocker))
			{
				aud_openLocker = defaultAudio,
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "yellowLocker.png")),
				defaultColor = new(0.99609375f, 0.91796875f, 0.01171875f) // Yellow
			};


			lockers.Add(new() { selection = locker, weight = 35 });

			texs = TextureExtensions.LoadTextureSheet(2, 1, BasePlugin.ModPath, "darkGreenLocker.png");
			

			// Dark green Locker (Block way)
			yield return "Creating dark green locker...";

			locker = new LockerObject(typeof(DarkGreenLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				defaultColor = new(0f, 0.1875f, 0.00390625f), // dark green
				minDistance = 28f,
				maxDistance = 40f
			};
			DarkGreenLocker.sprite = AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "stopBoard.png")), 35f);

			lockers.Add(new() { selection = locker, weight = 10 });

			texs = TextureExtensions.LoadTextureSheet(2, 1, BasePlugin.ModPath, "purpleLocker.png");

			yield return "Creating purple locker...";
			// Purple locker (Teleport)
			locker = new LockerObject(typeof(PurpleLocker))
			 {
				 aud_openLocker = defaultAudio,
				 openTex = texs[1],
				 closedTex = texs[0],
				 defaultColor = new(0.4609375f, 0f, 0.85546875f), // purple
				 minDistance = 50f,
				 maxDistance = 70f
			 };

			PurpleLocker.animation = new(TextureExtensions.LoadSpriteSheet(3, 3, 15f, BasePlugin.ModPath, "portal.png"), 0.7f);
			PurpleLocker.aud_tp = Resources.FindObjectsOfTypeAll<SoundObject>().First(x => x.name == "Teleport");
			PurpleLocker.aud_runningLoop = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "portal_loop.wav")), string.Empty, SoundType.Effect, Color.white);
			PurpleLocker.aud_runningLoop.subtitle = false;

			lockers.Add(new() { selection = locker, weight = 15 });
			yield return "Creating black locker...";

			texs = TextureExtensions.LoadTextureSheet(4, 3, BasePlugin.ModPath, "blackLocker.png");

			// Black locker (Steal)
			locker = new LockerObject(typeof(BlackLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[11],
				closedTex = texs[10],
				defaultColor = new(0.140625f, 0.140625f, 0.140625f), // black
				minDistance = 60f,
				maxDistance = 90f,
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "elephanthit.ogg")), "Vfx_Locker_elephant", SoundType.Voice, Color.white)
			};
			BlackLocker.texs = [.. texs.Take(10)];

			yield return "Creating brown locker...";

			lockers.Add(new() { selection = locker, weight = 10 });
			texs = TextureExtensions.LoadTextureSheet(2, 1, BasePlugin.ModPath, "brownLocker.png");
			// Brown Locker
			locker = new LockerObject(typeof(BrownLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				defaultColor = new(0.41796875f, 0.265625f, 0.1640625f),
				minDistance = 75f,
				maxDistance = 85f
			};

			BrownLocker.sprForLocker = AssetLoader.SpriteFromFile(Path.Combine(BasePlugin.ModPath, "brownLockerHud.png"), Vector2.one * 0.5f);
			lockers.Add(new() { selection = locker, weight = 15 });

			// Light Orange Locker
			yield return "Creating light orange locker...";
			texs = TextureExtensions.LoadTextureSheet(2, 2, BasePlugin.ModPath, "LightOrangeLockers.png");

			locker = new LockerObject(typeof(LightOrangeLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[3],
				closedTex = texs[2],
				defaultColor = new(0.98046875f, 0.23046875f, 0.015625f),
				minDistance = 75f,
				maxDistance = 85f
			};

			lockers.Add(new() { selection = locker, weight = 16 });

			locker = new LockerObject(typeof(DecoyLightOrangeLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				defaultColor = new(0.98046875f, 0.23046875f, 0.015625f),
				minDistance = 115f,
				maxDistance = 165f,
				aud_troll = GenericExtensions.FindResourceObject<LookAtGuy>().audBlindLoop,
				itemAmountToSteal = 2,
				decoyLaughCooldown = 14.5f,
			};

			lockers.Add(new() { selection = locker, weight = 10 });

			yield return "Creating aqua locker...";
			texs = TextureExtensions.LoadTextureSheet(2, 2, BasePlugin.ModPath, "aquaLocker.png");

			locker = new LockerObject(typeof(AquaLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[3],
				closedTex = texs[2],
				defaultColor = new(0.203125f, 0.671875f, 0.546875f),
				minDistance = 65f,
				maxDistance = 90f,
			};

			lockers.Add(new() { selection = locker, weight = 7 });

			locker = new LockerObject(typeof(DecoyAquaLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				defaultColor = new(0.203125f, 0.671875f, 0.546875f),
				minDistance = 65f,
				maxDistance = 90f,
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "ogNyanCat.wav")), "Vfx_Locker_NyanCat", SoundType.Voice, Color.white),
				itemAmountToSteal = 0,
			};

			lockers.Add(new() { selection = locker, weight = 14 });

			// Baldi Locker
			BaldiLocker.baldos = TextureExtensions.LoadTextureSheet(2, 2, BasePlugin.ModPath, "baldiLocker.png");

			locker = new LockerObject(typeof(BaldiLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = BaldiLocker.baldos[1],
				closedTex = BaldiLocker.baldos[0],
				defaultColor = new(0, 0.99609375f, 0.5625f),
				minDistance = 65f,
				maxDistance = 90f,
				aud_troll = GenericExtensions.FindResourceObjectByName<SoundObject>("Elv_Buzz"),
				itemAmountToSteal = 0,
			};

			lockers.Add(new() { selection = locker, weight = 20 });

			// Turquoise Locker
			texs = TextureExtensions.LoadTextureSheet(2, 1, BasePlugin.ModPath, "turquoiseLocker.png");

			locker = new LockerObject(typeof(TurquoiseLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				defaultColor = new(0, 0.828125f, 0.71484375f),
				minDistance = 75f,
				maxDistance = 90f,
			};

			TurquoiseLocker.sprite = AssetLoader.SpriteFromFile(Path.Combine(BasePlugin.ModPath, "turquoiseWater.png"), Vector2.one * 0.5f, 16f);

			lockers.Add(new() { selection = locker, weight = 20 });

			// *** items that opens lockers ***
			lockerAcceptableItems.Add(BasePlugin.lockpick.itemType);

			yield break;

		}
		const int enumeratorSize = 14;
		public static bool CanOpenLocker(Items i) => lockerAcceptableItems.Contains(i);

		readonly static HashSet<Items> lockerAcceptableItems = [];

		internal readonly static List<WeightedSelection<LockerObject>> lockers = [];

		internal const string textureColorProperty = "_TextureColor";

		internal static AssetManager man = new();
	}
}
