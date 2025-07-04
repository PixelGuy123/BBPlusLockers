using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BBPlusLockers.Lockers.DecoyLockers;
using BBPlusLockers.Lockers.HideableLockerVariants;
using BBPlusLockers.Plugin;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace BBPlusLockers.Lockers
{
	public static class LockerCreator
	{

		const string // constant strings for each floor
				F1 = "F1",
				F2 = "F2",
				F3 = "F3",
				F4 = "F4",
				F5 = "F5",
				END = "END"
			;

		internal static IEnumerator InitializeAssets() // Mods can *patch* this method with postfix to include their Items that lockers can accept
		{
			yield return enumeratorSize;


			LockerObject.baseLockerWhite = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "baseLockerSide.png"));
			var trueHideableLocker = GenericExtensions.FindResourceObject<HideableLocker>();
			HideableLockerVariants.HideableLocker.audSlam = trueHideableLocker.audSlam;
			SoundObject defaultAudio = GenericExtensions.FindResourceObjectByName<SoundObject>("Doors_Locker");
			Color blueLockerColor = new(0.015625f, 0.984375f, 0.984375f);
			HideableLockerVariants.HideableLocker.hideableLockerCanvasPre = GenericExtensions.FindResourceObject<HideableLocker>().hud;

			var texs = TextureExtensions.LoadTextureSheet(2, 1, BasePlugin.ModPath, "decoyBlueLocker.png");

			yield return "Creating decoy blue locker...";
			// Decoy Blue Locker
			var locker = new LockerObject(typeof(ClickableDecoyLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "trololo.wav")), "Vfx_Locker_trololo", SoundType.Voice, Color.white),
				defaultColor = blueLockerColor,
				itemAmountToSteal = 1
			};

			lockers[END].Add(new() { selection = locker, weight = 75 });
			lockers[F2].Add(new() { selection = locker, weight = 12 });
			lockers[F3].Add(new() { selection = locker, weight = 14 });
			lockers[F4].Add(new() { selection = locker, weight = 21 });
			lockers[F5].Add(new() { selection = locker, weight = 25 });

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

			lockers[F1].Add(new() { selection = locker, weight = 125 });
			lockers[F2].Add(new() { selection = locker, weight = 65 });


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

			lockers[F2].Add(new() { selection = locker, weight = 45 });
			lockers[F3].Add(new() { selection = locker, weight = 35 });
			lockers[F4].Add(new() { selection = locker, weight = 5 });
			lockers[F5].Add(new() { selection = locker, weight = 10 });


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

			lockers[END].Add(new() { selection = locker, weight = 50 });
			lockers[F2].Add(new() { selection = locker, weight = 25 });
			lockers[F3].Add(new() { selection = locker, weight = 12 });
			lockers[F4].Add(new() { selection = locker, weight = 12 });
			lockers[F5].Add(new() { selection = locker, weight = 13 });

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

			lockers[F2].Add(new() { selection = locker, weight = 25 });
			lockers[F3].Add(new() { selection = locker, weight = 12 });
			lockers[F4].Add(new() { selection = locker, weight = 12 });
			lockers[F5].Add(new() { selection = locker, weight = 6 });

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

			lockers[END].Add(new() { selection = locker, weight = 25 });
			lockers[F2].Add(new() { selection = locker, weight = 45 });
			lockers[F3].Add(new() { selection = locker, weight = 15 });
			lockers[F4].Add(new() { selection = locker, weight = 15 });
			lockers[F5].Add(new() { selection = locker, weight = 12 });

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

			lockers[END].Add(new() { selection = locker, weight = 35 });
			lockers[F3].Add(new() { selection = locker, weight = 16 });
			lockers[F4].Add(new() { selection = locker, weight = 12 });
			lockers[F5].Add(new() { selection = locker, weight = 11 });

			// Yellow Locker (store item)
			yield return "Creating yellow locker...";

			locker = new LockerObject(typeof(YellowLocker))
			{
				aud_openLocker = defaultAudio,
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "yellowLocker.png")),
				defaultColor = new(0.99609375f, 0.91796875f, 0.01171875f) // Yellow
			};

			lockers[END].Add(new() { selection = locker, weight = 15 });
			lockers[F1].Add(new() { selection = locker, weight = 75 });
			lockers[F2].Add(new() { selection = locker, weight = 45 });
			lockers[F3].Add(new() { selection = locker, weight = 35 });

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

			lockers[END].Add(new() { selection = locker, weight = 30 });
			lockers[F3].Add(new() { selection = locker, weight = 25 });
			lockers[F4].Add(new() { selection = locker, weight = 17 });
			lockers[F5].Add(new() { selection = locker, weight = 19 });

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

			lockers[END].Add(new() { selection = locker, weight = 22 });
			lockers[F4].Add(new() { selection = locker, weight = 20 });
			lockers[F5].Add(new() { selection = locker, weight = 17 });

			yield return "Creating black locker...";

			texs = TextureExtensions.LoadTextureSheet(11, 2, BasePlugin.ModPath, "blackLocker.png");

			// Black locker (Steal)
			locker = new LockerObject(typeof(BlackLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[texs.Length - 1],
				closedTex = texs[0],
				defaultColor = new(0.140625f, 0.140625f, 0.140625f), // black
				minDistance = 60f,
				maxDistance = 90f,
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "elephanthit.ogg")), "Vfx_Locker_elephant", SoundType.Voice, Color.white)
			};
			BlackLocker.lookingTextures = texs.Skip(10).Take(6);
			BlackLocker.fadeInTextures = texs.Take(10);
			BlackLocker.fadeOutScaredTextures = texs.Skip(16).Take(5);

			lockers[F3].Add(new() { selection = locker, weight = 15 });
			lockers[F4].Add(new() { selection = locker, weight = 6 });
			lockers[F5].Add(new() { selection = locker, weight = 2 });

			yield return "Creating brown locker...";

			texs = TextureExtensions.LoadTextureSheet(3, 1, BasePlugin.ModPath, "brownLocker.png");
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
			BrownLocker.sprForSight = texs[2];
			BrownLocker.sprForLocker = AssetLoader.SpriteFromFile(Path.Combine(BasePlugin.ModPath, "brownLockerHud.png"), Vector2.one * 0.5f);
			lockers[F1].Add(new() { selection = locker, weight = 25 });
			lockers[F2].Add(new() { selection = locker, weight = 25 });
			lockers[F3].Add(new() { selection = locker, weight = 15 });
			lockers[F4].Add(new() { selection = locker, weight = 35 });
			lockers[F5].Add(new() { selection = locker, weight = 15 });

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

			lockers[END].Add(new() { selection = locker, weight = 45 });
			lockers[F1].Add(new() { selection = locker, weight = 85 });
			lockers[F2].Add(new() { selection = locker, weight = 35 });
			lockers[F3].Add(new() { selection = locker, weight = 25 });
			lockers[F4].Add(new() { selection = locker, weight = 35 });
			lockers[F5].Add(new() { selection = locker, weight = 15 });

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
			DecoyLightOrangeLocker.gaugeSprite = GenericExtensions.FindResourceObject<LookAtGuy>().gaugeSprite;

			lockers[END].Add(new() { selection = locker, weight = 55 });
			lockers[F3].Add(new() { selection = locker, weight = 25 });
			lockers[F4].Add(new() { selection = locker, weight = 7 });
			lockers[F5].Add(new() { selection = locker, weight = 24 });

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
			AquaLocker.gaugeSprite = AssetLoader.SpriteFromFile(Path.Combine(BasePlugin.ModPath, "aquaLocker_icon.png"), Vector2.one * 0.5f, 1);

			lockers[END].Add(new() { selection = locker, weight = 25 });
			lockers[F3].Add(new() { selection = locker, weight = 14 });
			lockers[F4].Add(new() { selection = locker, weight = 15 });
			lockers[F5].Add(new() { selection = locker, weight = 12 });

			locker = new LockerObject(typeof(DecoyAquaLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = texs[1],
				closedTex = texs[0],
				defaultColor = new(0.203125f, 0.671875f, 0.546875f),
				minDistance = 65f,
				maxDistance = 90f,
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "ogNyanCat.wav")), "Vfx_Locker_NyanCat", SoundType.Effect, Color.white),
				itemAmountToSteal = 0,
			};

			lockers[END].Add(new() { selection = locker, weight = 50 });
			lockers[F3].Add(new() { selection = locker, weight = 40 });
			lockers[F4].Add(new() { selection = locker, weight = 35 });
			lockers[F5].Add(new() { selection = locker, weight = 20 });

			// Baldi Locker
			yield return "Creating Baldi Locker...";
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
			BaldiLocker.audOhHi = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "BAL_Locker.wav")), "Vfx_BAL_SingleHi", SoundType.Voice, Color.green);
			BaldiLocker.audPop = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "BAL_Locker_Pop.wav")), "Sfx_Effects_Pop", SoundType.Effect, Color.white);

			lockers[END].Add(new() { selection = locker, weight = 65 });
			lockers[F4].Add(new() { selection = locker, weight = 5 });
			lockers[F5].Add(new() { selection = locker, weight = 20 });

			// Turquoise Locker
			yield return "Creating turquoise locker...";
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

			lockers[END].Add(new() { selection = locker, weight = 35 });
			lockers[F3].Add(new() { selection = locker, weight = 35 });
			lockers[F4].Add(new() { selection = locker, weight = 16 });
			lockers[F5].Add(new() { selection = locker, weight = 22 });

			// *** items that opens lockers ***
			lockerAcceptableItems.Add(BasePlugin.lockpick.itemType);

			yield break;

		}
		const int enumeratorSize = 17;
		public static bool CanOpenLocker(Items i) => lockerAcceptableItems.Contains(i);

		readonly static HashSet<Items> lockerAcceptableItems = [];

		internal readonly static Dictionary<string, List<WeightedSelection<LockerObject>>> lockers = new() {
			{END, [new() { selection = null, weight = 100 }]}, // Null means the already default locker (base game blue locker
			{F1, [new() { selection = null, weight = 100 }]}, // Null means the already default locker (base game blue locker)
			{F2, [new() { selection = null, weight = 125 }] },
			{F3, [new() { selection = null, weight = 165 }] },
			{F4, [new() { selection = null, weight = 135 }] },
			{F5, [new() { selection = null, weight = 200 }] }
		};

		internal static bool TryGetLockers(LevelGenerationParameters lvlObj, out List<WeightedSelection<LockerObject>> lockersList)
		{
			lockersList = null;

			if (lvlObj == null || lvlObj is not CustomLevelGenerationParameters cLvl)
				return false;
			var modval = cLvl.GetCustomModValue(BasePlugin.guid, BasePlugin.customLockersDataKey);
			if (modval == null)
				return false;
			lockersList = modval as List<WeightedSelection<LockerObject>>;
			return true;
		}

		internal const string textureColorProperty = "_TextureColor";

		internal static AssetManager man = new();

		static T[] Skip<T>(this T[] ar, int count)
		{
			var newAr = new T[ar.Length - count];
			int index = count;
			for (int z = 0; z < newAr.Length; z++)
				newAr[z] = ar[index++];
			return newAr;
		}
		static T[] Take<T>(this T[] ar, int count) =>
			ar.Take(0, count);
		static T[] Take<T>(this T[] ar, int index, int count)
		{
			var newAr = new T[count];
			for (int z = 0; z < count; z++)
				newAr[z] = ar[index++];
			return newAr;
		}
	}
}
