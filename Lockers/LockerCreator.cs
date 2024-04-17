using BBPlusLockers.Plugin;
using System.Collections.Generic;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using System.IO;
using UnityEngine;
using System.Linq;
using PixelInternalAPI.Classes;

namespace BBPlusLockers.Lockers
{
	public static class LockerCreator
	{

		internal static void InitializeAssets() // Mods can *patch* this method with postfix to include their Items that lockers can accept
		{
			// no sprite billboard
			var baseSprite = new GameObject("SpriteNoBillBoard").AddComponent<SpriteRenderer>();
			baseSprite.material = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "SpriteStandard_NoBillboard" && x.GetInstanceID() > 0);
			baseSprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			baseSprite.receiveShadows = false;

			baseSprite.gameObject.layer = LayerStorage.billboardLayer;
			Object.DontDestroyOnLoad(baseSprite.gameObject);
			baseSprite.gameObject.SetActive(false);
			man.Add("SpriteNoBillboardTemplate", baseSprite);

			// sprite billboard
			baseSprite = new GameObject("SpriteBillBoard").AddComponent<SpriteRenderer>();
			baseSprite.material = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name == "SpriteStandard_Billboard" && x.GetInstanceID() > 0);
			baseSprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			baseSprite.receiveShadows = false;

			baseSprite.gameObject.layer = LayerStorage.billboardLayer;
			Object.DontDestroyOnLoad(baseSprite.gameObject);
			baseSprite.gameObject.SetActive(false);
			man.Add("SpriteBillboardTemplate", baseSprite);



			lockers.Add(new() { selection = null, weight = 55 }); // Null means the already default locker (hideablelocker)

			SoundObject defaultAudio = Resources.FindObjectsOfTypeAll<SoundObject>().First(x => x.name == "Doors_Locker");
			// Green Locker
			var locker = new LockerObject(typeof(GreenLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "greenLocker_open.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "greenLocker.png")),
				defaultColor = Color.green
			};

			lockers.Add(new() { selection = locker, weight = 25 });

			locker = new LockerObject(typeof(AcceptorDecoyLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "decoy_greenLocker_open.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "decoy_greenLocker.png")),
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "HA_HA.wav")), "Vfx_Locker_HAHA", SoundType.Voice, Color.white),
				defaultColor = Color.green
			};

			lockers.Add(new() { selection = locker, weight = 25 });


			// Decoy Blue Locker
			locker = new LockerObject(typeof(ClickableDecoyLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "decoy_blueLocker_open.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "decoy_blueLocker.png")),
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "trololo.wav")), "Vfx_Locker_trololo", SoundType.Voice, Color.white),
				useDefaultColor = false,
				itemAmountToSteal = 2
			};

			lockers.Add(new() { selection = locker, weight = 15 });

			// Dark blue locker (PULL FORCE)

			locker = new LockerObject(typeof(DarkBlueLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "darkBlueLocker_open.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "darkBlueLocker.png")),
				defaultColor = new(0.01171875f, 0.01171875f, 0.99609375f) // dark blue
			};
			DarkBlueLocker.aud_vacuumStart = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "vacuum_start.wav")), "Vfx_Locker_vacuum", SoundType.Voice, Color.white);
			DarkBlueLocker.aud_vacuumLoop = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "vacuum_loop.wav")), "Vfx_Locker_vacuum", SoundType.Voice, Color.white);

			lockers.Add(new() { selection = locker, weight = 35 });

			locker = new LockerObject(typeof(AcceptorDecoyLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "decoydarkBlueLocker_open.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "decoydarkBlueLocker.png")),
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "deepHA_HA.wav")), "Vfx_Locker_deepHAHA", SoundType.Voice, Color.white),
				defaultColor = new(0.01171875f, 0.01171875f, 0.99609375f), // dark blue
				itemAmountToSteal = 2
			};

			lockers.Add(new() { selection = locker, weight = 25 });

			// Orange Locker (Push force)

			var orangeLockerAudio = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "deepslam.wav")), "Vfx_Locker_deepSLAM", SoundType.Voice, Color.white);
			locker = new LockerObject(typeof(OrangeLocker))
			{
				aud_openLocker = orangeLockerAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "orangeLocker_open0.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "orangeLocker.png")),
				defaultColor = new(0.99609375f, 0.61328125f, 0.04296875f) // orange
			};

			OrangeLocker.openTexs = new Texture2D[4];
			for (int i = 0; i < OrangeLocker.openTexs.Length; i++)
				OrangeLocker.openTexs[i] = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, $"orangeLocker_open{i + 1}.png"));


			lockers.Add(new() { selection = locker, weight = 15 });

			locker = new LockerObject(typeof(DecoyOrangeLocker))
			{
				aud_openLocker = orangeLockerAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "decoyOrangeLocker_open.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "decoyOrangeLocker.png")),
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "heheheha.wav")), "Vfx_Locker_heheha", SoundType.Voice, Color.white),
				defaultColor = new(0.99609375f, 0.61328125f, 0.04296875f), // orange
				itemAmountToSteal = 4
			};

			OrangeLocker.openTexs = new Texture2D[4];
			for (int i = 0; i < OrangeLocker.openTexs.Length; i++)
				OrangeLocker.openTexs[i] = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, $"orangeLocker_open{i + 1}.png"));


			lockers.Add(new() { selection = locker, weight = 5 });

			// Yellow Locker (store item)

			locker = new LockerObject(typeof(YellowLocker))
			{
				aud_openLocker = defaultAudio,
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "yellowLocker.png")),
				defaultColor = new(0.99609375f, 0.91796875f, 0.01171875f) // Yellow
			};


			lockers.Add(new() { selection = locker, weight = 35 });

			// Dark green Locker (Block way)

			locker = new LockerObject(typeof(DarkGreenLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "darkGreenLocker_open.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "darkGreenLocker.png")),
				defaultColor = new(0f, 0.1875f, 0.00390625f), // dark green
				minDistance = 28f,
				maxDistance = 40f
			};
			DarkGreenLocker.sprite = AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "stopBoard.png")), 35f);

			lockers.Add(new() { selection = locker, weight = 10 });

			// Purple locker (Teleport)
			locker = new LockerObject(typeof(PurpleLocker))
			 {
				 aud_openLocker = defaultAudio,
				 openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "purpleLocker_open0.png")),
				 closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "purpleLocker.png")),
				 defaultColor = new(0.4609375f, 0f, 0.85546875f), // purple
				 minDistance = 50f,
				 maxDistance = 70f
			 };
			Sprite[] sprites = new Sprite[10]; // Custom Sprite Animator ends one frame earlier, so I had to leave a null one >:(
			for (int i = 0; i < sprites.Length - 1; i++)
				sprites[i] = AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, $"portal_{i}.png")), 15f);

			PurpleLocker.animation = new(sprites, 0.7f);
			PurpleLocker.aud_tp = Resources.FindObjectsOfTypeAll<SoundObject>().First(x => x.name == "Teleport");

			lockers.Add(new() { selection = locker, weight = 15 });

			// Black locker (Steal)
			locker = new LockerObject(typeof(BlackLocker))
			{
				aud_openLocker = defaultAudio,
				openTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "blackLocker_open.png")),
				closedTex = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, "blackLocker_10.png")),
				defaultColor = new(0.140625f, 0.140625f, 0.140625f), // black
				minDistance = 60f,
				maxDistance = 90f,
				aud_troll = ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromFile(Path.Combine(BasePlugin.ModPath, "elephanthit.ogg")), "Vfx_Locker_elephant", SoundType.Voice, Color.white)
			};
			BlackLocker.texs = new Texture2D[10];
			for (int i = 0; i < BlackLocker.texs.Length; i++)
				BlackLocker.texs[i] = AssetLoader.TextureFromFile(Path.Combine(BasePlugin.ModPath, $"blackLocker_{i}.png"));

			lockers.Add(new() { selection = locker, weight = 25 });

			// *** items that opens lockers ***
			lockerAcceptableItems.Add(BasePlugin.lockpick.itemType);

		}
		public static bool CanOpenLocker(Items i) => lockerAcceptableItems.Contains(i);

		readonly static HashSet<Items> lockerAcceptableItems = [];

		internal readonly static List<WeightedSelection<LockerObject>> lockers = [];

		internal const string textureColorProperty = "_TextureColor";

		internal static AssetManager man = new();
	}
}
