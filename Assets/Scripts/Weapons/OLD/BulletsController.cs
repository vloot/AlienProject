using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletsController : MonoBehaviour
{
    [Header("Bullets config")]
    [SerializeField] private Transform bulletParent;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifetime;

    [Header("Collision")]
    [SerializeField] private LayerMask destructionLayerMask;


    private float _timeSinceLastBullet;

    private ObjectPool<Bullet> bulletsPool;

    private List<Bullet> firedBullets;


    private void Awake()
    {
        bulletsPool = new ObjectPool<Bullet>(20, bullet, bulletParent, 20);
        firedBullets = new List<Bullet>();
    }

    private void Start()
    {
        Bullet.OnCollision += OnCollision;
    }

    public void Fire(Vector3 spawnPosition, Vector3 direction, Quaternion rotation)
    {
        var time = Time.time;

        if (time - _timeSinceLastBullet <= fireRate) return;

        // fire a bullet
        var bullet = bulletsPool.Spawn(spawnPosition);
        var randomSpeedOffset = Random.Range(.85f, 1.25f);
        bullet.Fire(direction * bulletSpeed * randomSpeedOffset, rotation, time);

        firedBullets.Add(bullet);

        // update time
        _timeSinceLastBullet = time;
    }

    private void FixedUpdate()
    {
        var time = Time.time;

        for (int i = 0; i < firedBullets.Count; i++)
        {
            var bullet = firedBullets[i];

            if (time - bullet.creationTime >= bulletLifetime)
            {
                DespawnBullet(bullet);
                continue;
            }
            else if (bullet.isFired)
            {
                bullet.transform.position += bullet.direction * Time.deltaTime;
            }
        }
    }

    private void OnCollision(Bullet bullet, Collision other)
    {
        if (destructionLayerMask.Contains(other.gameObject.layer))
        {
            // enemy or wall hit, despawn
            bullet.isFired = false;
            DespawnBullet(bullet);
        }
    }

    private void DespawnBullet(Bullet bullet)
    {
        firedBullets.Remove(bullet);
        bulletsPool.Despawn(bullet);
    }
}
