using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    public WeaponType weaponType;
    public float fireRatePerSecond;
    public Vector2 spread;

    public int damage;
}

