using System.Collections.Generic;
using BBPlusLockers.Structures;
using HarmonyLib;
using MTM101BaldAPI;
using PixelInternalAPI.Extensions;
using UnityEngine;

namespace BBPlusLockers.Plugin;

public partial class ExtraLockersPlugin
{
    readonly internal static HashSet<Transform> lockerPrefabs = [];
    public void GetLockerAssets()
    {
        var lockerBld = GenericExtensions.FindResourceObject<Structure_Lockers>();
        assetMan.Add("LockerPrefab", lockerBld.lockerPre);

        lockerPrefabs.Add(lockerBld.lockerPre.transform);
        lockerPrefabs.Add(lockerBld.hideLockerPre.transform);

        var builder = new GameObject("CustomLocker_Structure").AddComponent<Structure_CustomLockers>();
        builder.gameObject.ConvertToPrefab(true);
        assetMan.Add("CustomLockerBld", builder);
    }

    bool AddCustomLockerToLevelObject(LevelObject ld, string lvlname)
    {
        if (!customLockerParameters.TryGetValue(lvlname, out var parameters))
            return false;

        ld.forcedStructures = ld.forcedStructures.AddToArray(
            new()
            {
                prefab = assetMan.Get<Structure_CustomLockers>("CustomLockerBld"),
                parameters = parameters
            }
        );
        return true;
    }
}