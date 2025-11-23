using System.IO;
using BBPlusLockers.Plugin;
using BBPlusLockers.Structures;
using MTM101BaldAPI.AssetTools;
using PlusLevelStudio;
using PlusLevelStudio.Editor;
using UnityEngine;

namespace BBPlusLockers.Compats;

internal static class EditorIntegration
{
    private static AssetManager _editorAssetMan;

    internal static void Initialize(AssetManager man)
    {
        LoadEditorAssets();
        InitializeVisuals(man);
        EditorInterfaceModes.AddModeCallback(InitializeTools);
    }

    private static void LoadEditorAssets()
    {
        _editorAssetMan = new AssetManager();
        string editorUIPath = Path.Combine(ExtraLockersPlugin.ModPath, "Editor");

        // Load all general UI sprites
        string[] files = Directory.GetFiles(editorUIPath);
        foreach (string file in files)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            // Debug.Log($"Adding icon \'UI/{name}\'");
            _editorAssetMan.Add("UI/" + name, AssetLoader.SpriteFromTexture2D(AssetLoader.TextureFromFile(file), 40f));
        }
    }

    private static void InitializeVisuals(AssetManager man)
    {
        // ** Global Structures **
        LevelStudioPlugin.Instance.structureTypes.Add("CustomLockers", typeof(OutsideBoxLocation)); // It does nothing, so it's a good stub
    }


    private static void InitializeTools(EditorMode mode, bool isVanillaCompliant)
    {

        // Outside Tool
        mode.globalRandomStructures.Add(new()
        {
            nameKey = $"Ed_GlobalStructure_CustomLockers_Title",
            descKey = $"Ed_GlobalStructure_CustomLockers_Desc",
            structureToSpawn = "CustomLockers",
            settingsPageType = typeof(OutsideBoxUIHandler),
            settingsPagePath = Structure_CustomLockers.GetJSONUIPath()
        });
    }

    private static Sprite GetSprite(string key1, string key2)
    {
        var spr = _editorAssetMan.ContainsKey(key1) ? _editorAssetMan.Get<Sprite>(key1) : _editorAssetMan.Get<Sprite>(key2);
        // Debug.Log($"Getting sprite: {(_editorAssetMan.ContainsKey(key1) ? key1 : key2)}");
        return spr;
    }

    static GameObject AddStructureGenericVisual(string key, GameObject obj, params System.Type[] exceptions)
    {
        GameObject gameObject = EditorInterface.CloneToPrefabStripMonoBehaviors(obj, exceptions);
        gameObject.name = gameObject.name.Replace("_Stripped", "_GenericStructureVisual");
        EditorRendererContainer editorRendererContainer = gameObject.gameObject.AddComponent<EditorRendererContainer>();
        editorRendererContainer.AddRendererRange(gameObject.GetComponentsInChildren<Renderer>(), "none");
        gameObject.gameObject.AddComponent<EditorDeletableObject>().renderContainer = editorRendererContainer;
        gameObject.layer = 13;
        LevelStudioPlugin.Instance.genericStructureDisplays.Add(key, gameObject);
        return gameObject;
    }
}