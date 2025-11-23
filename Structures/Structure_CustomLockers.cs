using System.Collections.Generic;
using System.IO;
using BBPlusLockers.Plugin;
using UnityEngine;

namespace BBPlusLockers.Structures;

public class Structure_CustomLockers : StructureBuilder
{
    public static string GetJSONUIPath() => Path.Combine(ExtraLockersPlugin.ModPath, "Editor", "UI", "CustomLockersUI.json");
    public readonly static List<GameObject> replaceableLockers = [];

    public override void Finished()
    {
        base.Finished();
        replaceableLockers.Clear();
    }
}