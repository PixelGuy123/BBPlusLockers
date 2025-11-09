using System.Collections;
using System.IO;
using BepInEx;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using PixelInternalAPI;
using PixelInternalAPI.Extensions;
namespace BBPlusLockers.Plugin;

public partial class ExtraLockersPlugin
{
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
            .SetMeta(MTM101BaldAPI.Registers.ItemFlags.None, ["StackableItems_NotAllowStacking"])
            .Build();
        ((ITM_Acceptable)item.item).item = item.itemType;
        ((ITM_Acceptable)item.item).layerMask = GenericExtensions.FindResourceObjectByName<LayerMaskObject>("PlayerClickLayerMask");

        lockpick = item;
        lockpick.AddKeyTypeItem();

        ResourceManager.AddWeightedItemToCrazyMachine(new() { selection = lockpick, weight = 55 });
    }

    void RegisterGenerationChanges(string lvlname, int lvlNum, SceneObject sco)
    {

    }
}