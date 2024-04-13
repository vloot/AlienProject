using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    public WeaponStats weaponStats;

    [SerializeField] protected LayerMask destructionLayerMask;
    [SerializeField] protected WeaponsManager weaponsManager;

    protected float _timeSinceLastBullet;

    public abstract void Use(Vector3 position, Vector3 forward, Quaternion rotation);

    public bool CanFire(float time)
    {
        return time - _timeSinceLastBullet >= weaponStats.fireRatePerSecond;
    }
}
