using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private WeaponsManager weaponsManager;

    [Header("Other")]
    [SerializeField] private LayerMask projectilesMask;
    [SerializeField] private Transform playerTransform;

    private List<AbstractEnemy> enemies;
    private Dictionary<GameObject, AbstractEnemy> spawnedEnemiesDict;
    private int enemiesCount;
    private bool _updateEven;

    private void Awake()
    {
        enemies = new List<AbstractEnemy>();
        spawnedEnemiesDict = new Dictionary<GameObject, AbstractEnemy>();

        enemies = GetComponentsInChildren<AbstractEnemy>().ToList();
        enemiesCount = enemies.Count;

        print("enemiesCount = " + enemiesCount);

        foreach (var item in enemies)
        {
            spawnedEnemiesDict[item.GetGameObject()] = item;
            item.enemyManager = this;
        }
    }

    private void FixedUpdate()
    {
        if (Time.frameCount % 2 == 0) return; // only update path every other frame

        var playerPos = playerTransform.position;
        var time = Time.time;

        for (int i = 0; i < enemiesCount; i++)
        {
            if (_updateEven && i % 2 == 0)
            {
                if (enemies[i].agent.enabled)
                    enemies[i].SetDestination(playerPos, time);
            }
            else if (!_updateEven && i % 2 != 0)
            {
                if (enemies[i].agent.enabled)
                    enemies[i].SetDestination(playerPos, time);
            }
        }

        _updateEven ^= true;
    }

    public void RegisterHit(GameObject enemyGameObject)
    {
        if (!spawnedEnemiesDict.ContainsKey(enemyGameObject)) return;

        ProcessHit(spawnedEnemiesDict[enemyGameObject]);
    }

    public void RegisterHit(Collision other, AbstractEnemy enemy)
    {
        if (!projectilesMask.Contains(other.gameObject.layer)) return;

        ProcessHit(enemy);
    }

    private void ProcessHit(AbstractEnemy enemy)
    {
        var weapon = weaponsManager.GetCurrentWeapon();
        enemy.RegisterHit(weapon.weaponStats.damage);
    }
}
