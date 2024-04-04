using UnityEngine;

public class ProjectileWeapon : AbstractWeapon
{
    public ProjectileStats projectileStats;

    [Header("ObjectPool setup")]
    [SerializeField] private int poolStartSize;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnParent;

    private ProjectileManager projectileManager;

    private void Awake()
    {
        projectileManager = new ProjectileManager(poolStartSize, projectilePrefab, spawnParent, destructionLayerMask);
    }

    public override void Use(Vector3 position, Vector3 direction, Quaternion rotation)
    {
        var time = Time.time;

        if (time - _timeSinceLastBullet <= weaponStats.fireRatePerSecond) return;

        // fire a bullet
        var projectile = projectileManager.SpawnProjectile(position);
        var randomSpeedOffset = Random.Range(.85f, 1.25f);
        projectile.Launch(direction * projectileStats.projectileSpeed * randomSpeedOffset, rotation, time);

        // update time
        _timeSinceLastBullet = time;
    }

    private void FixedUpdate()
    {
        projectileManager.UpdateProjectiles(projectileStats);
    }
}
