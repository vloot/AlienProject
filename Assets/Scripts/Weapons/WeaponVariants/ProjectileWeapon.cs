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

        var randAngle = Random.Range(weaponStats.spread.x, weaponStats.spread.y) * Mathf.Deg2Rad;
        var newRotation = Quaternion.Euler(0, randAngle, 0);
        var rotatedVector = newRotation * direction;

        // fire a bullet
        var projectile = projectileManager.SpawnProjectile(position);
        projectile.Launch(rotatedVector * projectileStats.projectileSpeed, rotation, time);

        // update time
        _timeSinceLastBullet = time;
    }

    private void FixedUpdate()
    {
        projectileManager.UpdateProjectiles(projectileStats);
    }
}
