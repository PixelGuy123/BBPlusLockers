using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using PixelInternalAPI;
using PixelInternalAPI.Extensions;
namespace BBPlusLockers.Plugin;

public partial class ExtraLockersPlugin
{
    // ***** FIELDS ******
    internal static ItemObject lockpick; // Will be useful for custom lockers to check for the lockpick;
    readonly Dictionary<string, int> lockpickWeightsByLevel = new()
        {
            {"F1", 35},
            {"F2", 55},
            {"F3", 65},
            {"F4", 35},
            {"F5", 50},
            {"END", 75}
        };
    readonly Dictionary<string, int> shopLockpickWeightsByLevel = new()
        {
            {"F1", 45},
            {"F2", 65},
            {"F3", 25},
            {"F4", 30},
            {"F5", 15},
            {"END", 30}
        };
    readonly Dictionary<string, StructureParameters> customLockerParameters = new()
    {
        { "F1", new() { chance = [0.05f] } },
        { "F2", new() { chance = [0.06f] } },
        { "F3", new() { chance = [0.1f] } },
        { "F4", new() { chance = [0.17f] } },
        { "F5", new() { chance = [0.25f] } },
        { "END", new() { chance = [0.3f] } },
    };

    // ******** METHODS *********
    void CreateLockpick()
    {
        var item = new ItemBuilder(Info)
           .SetEnum("Lockpick")
           .SetShopPrice(350)
           .SetGeneratorCost(20)
           .SetItemComponent<ITM_Acceptable>()
           .SetSprites(AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(ModPath, "lockpick_small.png")), 1f),
           AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(Path.Combine(ModPath, "lockpick.png")), 50f))
           .SetNameAndDescription("LPC_Name", "LPC_Desc")
           .SetMeta(MTM101BaldAPI.Registers.ItemFlags.None, ["StackableItems_NotAllowStacking"])
           .Build();
        ((ITM_Acceptable)item.item).item = item.itemType;
        ((ITM_Acceptable)item.item).layerMask = GenericExtensions.FindResourceObjectByName<LayerMaskObject>("PlayerClickLayerMask");

        lockpick = item;
        lockpick.AddKeyTypeItem();

        ResourceManager.AddWeightedItemToCrazyMachine(new() { selection = lockpick, weight = 55 });
    }

    bool AddLockpickToLevelObject(LevelObject ld, string lvlname)
    {
        if (!lockpickWeightsByLevel.TryGetValue(lvlname, out int weight))
            return false;

        ld.potentialItems = ld.potentialItems.AddToArray(new() { selection = lockpick, weight = weight });
        return true;
    }
}