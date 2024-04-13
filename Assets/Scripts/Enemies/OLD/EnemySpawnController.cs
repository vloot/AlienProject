using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float spawnRadius;

    private List<AbstractEnemy> enemies;
    private int enemiesCount;

    private void Start()
    {
        enemies = GetComponentsInChildren<AbstractEnemy>().ToList();
        enemiesCount = enemies.Count;
        print("enemiesCount = " + enemiesCount);
    }

    private bool updateEven;

    private void FixedUpdate()
    {
        if (Time.frameCount % 2 == 0) return; // only update path every other frame

        var playerPos = playerTransform.position;
        var time = Time.time;

        for (int i = 0; i < enemiesCount; i++)
        {
            if (updateEven && i % 2 == 0)
            {
                if (enemies[i].agent.enabled)
                    enemies[i].SetDestination(playerPos, time);
            }
            else if (!updateEven && i % 2 != 0)
            {
                if (enemies[i].agent.enabled)
                    enemies[i].SetDestination(playerPos, time);
            }
        }

        updateEven ^= true;
    }

    private Vector3[] GetSpawnPoints(int amount)
    {
        var points = new Vector3[amount];

        for (int i = 0; i < amount; i++)
        {
            var angle = Random.Range(1, 360);
            var x = transform.position.x + spawnRadius * Mathf.Cos(angle);
            var y = transform.position.z + spawnRadius * Mathf.Sin(angle);

            points[i] = new Vector3(x, 0, y);
        }

        return points;
    }
}
