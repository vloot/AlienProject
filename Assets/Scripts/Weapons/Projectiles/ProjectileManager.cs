using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager
{
    private ObjectPool<AbstractProjectile> _objectPool;
    private List<AbstractProjectile> _spawnedProjectiles;
    private LayerMask _destructionLayerMask;

    public ProjectileManager(int poolStartSize, GameObject prefab, Transform spawnParent, LayerMask destructionMask)
    {
        _objectPool = new ObjectPool<AbstractProjectile>(poolStartSize, prefab, spawnParent, bulletSpawnedAction: (p) => p.manager = this);
        _spawnedProjectiles = new List<AbstractProjectile>(poolStartSize);
        _destructionLayerMask = destructionMask;

    }

    public AbstractProjectile SpawnProjectile(Vector3 pos)
    {
        var projectile = _objectPool.Spawn(pos);

        if (!_spawnedProjectiles.Contains(projectile))
        {
            _spawnedProjectiles.Add(projectile);
        }

        return projectile;
    }

    public void DespawnProjectile(AbstractProjectile projectile)
    {
        // spawnedProjectiles.Remove(projectile);
        projectile.isFired = false;
        _objectPool.Despawn(projectile);
    }

    internal void UpdateProjectiles(ProjectileStats stats)
    {
        var deltaTime = Time.deltaTime;
        var time = Time.time;
        for (int i = 0; i < _spawnedProjectiles.Count; i++)
        {
            if (!_spawnedProjectiles[i].isFired) continue;

            if (time - _spawnedProjectiles[i].creationTime >= stats.projectileLifetime)
            {
                _spawnedProjectiles[i].isFired = false;
                // modifying the list while iterating it, lol
                DespawnProjectile(_spawnedProjectiles[i]);
                continue;
            }

            _spawnedProjectiles[i].UpdateProjectile(deltaTime);
        }
    }

    public void OnCollision(AbstractProjectile projectile, Collision other)
    {
        if (_destructionLayerMask.Contains(other.gameObject.layer))
        {
            // enemy or wall hit, despawn
            projectile.isFired = false;
            DespawnProjectile(projectile);
        }
    }
}
