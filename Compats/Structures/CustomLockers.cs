using System.Collections.Generic;
using System.IO;
using BBPlusLockers.Lockers;
using PlusLevelStudio.Editor;
using PlusLevelStudio.Editor.GlobalSettingsMenus;
using PlusStudioLevelFormat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BBPlusLockers.Compats.Structures;

public class CustomLockerPage(TextMeshProUGUI lockerDisplay, MenuToggle toggle, string lockerReference)
{
    public CustomLockerPage(string lockerReference, int weight) : this(null, null, lockerReference)
    {
        Weight = weight;
    }

    public MenuToggle Toggle = toggle;
    public TextMeshProUGUI LockerDisplay = lockerDisplay;
    public string Locker = lockerReference;
    public int Weight = 100;

    public void Switch(bool on)
    {
        Toggle.enabled = on;
        LockerDisplay.enabled = on;
    }

    public WeightedPrefab WeightedPrefab => new() { prefab = Locker, weight = Weight };
}

public class CustomLockersUIHandler : GlobalStructureUIHandler
{
    readonly List<CustomLockerPage> pages = [];
    public TextMeshProUGUI chanceToSpawnLabel;

    // Structure
    public CustomLockersLocation CustomLockers => structure == null ? null : (CustomLockersLocation)structure;

    public override bool GetStateBoolean(string key) => false;

    public override void OnElementsCreated()
    {
        outsideLightColor_R = transform.Find("OutsideColor_R").GetComponent<TextMeshProUGUI>();
        outsideLightColor_G = transform.Find("OutsideColor_G").GetComponent<TextMeshProUGUI>();
        outsideLightColor_B = transform.Find("OutsideColor_B").GetComponent<TextMeshProUGUI>();
        outsideStrength = transform.Find("OutsideStrength").GetComponent<TextMeshProUGUI>();
        shouldIncludeGrass_tick = transform.Find("includeGrassTick").GetComponent<MenuToggle>();
        isOnLastFloor_tick = transform.Find("lastFloorTick").GetComponent<MenuToggle>();
        hexDisplay = transform.Find("OutsideColorDisplay").GetComponent<Image>();
    }

    public override void PageLoaded(StructureLocation structure)
    {
        base.PageLoaded(structure);

    }

    public override void SendInteractionMessage(string message, object data = null)
    {
        byte value;
        switch (message)
        {

            case "includeLastFloor":
                if (data is bool toggle2)
                {
                    OutsideStructure.isAtLastFloor = toggle2;
                }
                PageLoaded(structure);
                break;
        }
    }
}

public class CustomLockersLocation : RandomStructureLocation
{
    // Unimplemented stuff
    public override void AddStringsToCompressor(StringCompressor compressor) { }
    public override void CleanupVisual(GameObject visualObject) { }
    public override GameObject GetVisualPrefab() => null;
    public override void InitializeVisual(GameObject visualObject) { }
    public override void ShiftBy(Vector3 worldOffset, IntVector2 cellOffset, IntVector2 sizeDifference) { }
    public override bool ValidatePosition(EditorLevelData data) => true;
    public override void UpdateVisual(GameObject visualObject) { }

    // Actually used
    // *Fields*
    public List<CustomLockerPage> availablePages = [];
    public float chanceToSpawnLocker = 0.5f;
    // *Methods*
    public override RandomStructureInfo CompileIntoRandom(EditorLevelData data, BaldiLevel level)
    {
        List<WeightedPrefab> objs = new(availablePages.Count);
        for (int i = 0; i < objs.Count; i++)
            objs[i] = availablePages[i].WeightedPrefab;

        return new()
        {
            type = type,
            info = new StructureParameterInfo()
            {
                chance = [chanceToSpawnLocker],
                prefab = objs
            }
        };
    }

    public override void ReadInto(EditorLevelData data, BinaryReader reader, StringCompressor compressor)
    {
        _ = reader.ReadByte(); // version
        chanceToSpawnLocker = reader.ReadSingle();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
            availablePages.Add(new(reader.ReadString(), reader.ReadInt32()));
    }


    public override void Write(EditorLevelData data, BinaryWriter writer, StringCompressor compressor)
    {
        writer.Write((byte)0);
        writer.Write(chanceToSpawnLocker);
        writer.Write(availablePages.Count);
        foreach (var page in availablePages)
        {
            writer.Write(page.Locker);
            writer.Write(page.Weight);
        }
    }
}