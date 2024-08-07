﻿using BBPlusLockers.Plugin;
using System.Collections.Generic;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections;
using PixelInternalAPI.Extensions;

namespace BBPlusLockers.Lockers
{
	public static class LockerCreator
	{

		internal static IEnumerator InitializeAssets() // Mods can *patch* this method with postfix to include their Items that lockers can accept
		{
			yield return enumeratorSize;

			LockerObject.baseLockerWhite = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "baseLockerSide.png"));

			yield return "Creating green locker...";
			lockers.Add(new() { selection = null, weight = 55 }); // Null means the already default locker (hideablelocker)

			Texture2D[] texs = TextureExtensions.LoadTextureSheet(2, 2, BasePlugin.ModPath, "greenLockers.png");

			SoundObject defaultAudio = Resources.FindObjectsOfTypeAll<SoundObject>().First(x => x.name == "Doors_Locker");
			// Green Locker
			var locker = new LockerObject(typeof(GreenLocker))
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
				defaultColor = Color.green
			};

			lockers.Add(new() { selection = locker, weight = 25 });

			texs = TextureExtensions.LoadTextureSheet(2, 1, BasePlugin.ModPath, "decoyBlueLocker.png");

			yield return "Creating decoy blue locker...";
			// Decoy Blue Locker
			locker = new LockerObject(typeof(ClickableDecoyLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "trololo.wav")), "Vfx_Locker_trololo", SoundType.Voice, Color.white),
				useDefaultColor = false,
				itemAmountToSteal = 2
			};

			lockers.Add(new() { selection = locker, weight = 15 });

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
			locker = new LockerObject(typeof(AcceptorDecoyLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[3],
				closedTex = texs[2],
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "deepHA_HA.wav")), "Vfx_Locker_deepHAHA", SoundType.Voice, Color.white),
				defaultColor = new(0.01171875f, 0.01171875f, 0.99609375f), // dark blue
				itemAmountToSteal = 2
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
				itemAmountToSteal = 4
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

			lockers.Add(new() { selection = locker, weight = 15 });
			yield return "Creating black locker locker...";

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

			lockers.Add(new() { selection = locker, weight = 10 });

			// *** items that opens lockers ***
			lockerAcceptableItems.Add(BasePlugin.lockpick.itemType);

			yield break;

		}
		const int enumeratorSize = 11;
		public static bool CanOpenLocker(Items i) => lockerAcceptableItems.Contains(i);

		readonly static HashSet<Items> lockerAcceptableItems = [];

		internal readonly static List<WeightedSelection<LockerObject>> lockers = [];

		internal const string textureColorProperty = "_TextureColor";

		internal static AssetManager man = new();
	}
}
