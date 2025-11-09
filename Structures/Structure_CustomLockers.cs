using System.Collections.Generic;
using UnityEngine;

namespace BBPlusLockers.Structures;

public class Structure_CustomLockers : StructureBuilder
{
    public static List<GameObject> replaceableLockers = [];

    public override void Finished()
    {
        base.Finished();
        replaceableLockers.Clear();
    }
}