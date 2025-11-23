using BBPlusLockers.Plugin;
using MTM101BaldAPI;
using UnityEngine;

namespace BBPlusLockers.Lockers;

public abstract class BaseLocker : EnvironmentObject
{
    [SerializeField]
    protected MeshRenderer lockerRenderer;
    public Texture2D MainTexture { get => lockerRenderer.materials[0].mainTexture as Texture2D; protected set => lockerRenderer.materials[0].mainTexture = value; }
    public Color SideColor { get => lockerRenderer.materials[1].GetColor("_TextureColor"); protected set => lockerRenderer.materials[1].SetColor("_TextureColor", value); }
    public static L CreateLockerPrefab<L>() where L : BaseLocker
    {
        var newLocker = Instantiate(ExtraLockersPlugin.assetMan.Get<MeshRenderer>("LockerPrefab"));
        newLocker.gameObject.ConvertToPrefab(true);
        newLocker.name = nameof(L);
        var locComp = newLocker.gameObject.AddComponent<L>();
        locComp.lockerRenderer = newLocker;
        locComp.SideColor = locComp.GetDefaultLockerColor();
        locComp.SetupLockerPrefab();

        return locComp;
    }
    public abstract void SetupLockerPrefab();
    public abstract Color GetDefaultLockerColor();

    /// <summary>
    /// When the <see cref="BaseLocker"/> overrides the locker in the current position.
    /// </summary>
    public virtual void OnLockerOverride() { }
}