using UnityEngine;
using UnityEngine.AI;

public class AbstractEnemy : MonoBehaviour, IPoolObject<AbstractEnemy>
{
    [Header("Components")]
    [SerializeField] public EnemyManager enemyManager;

    [Header("Stats")]
    [SerializeField] private float updateFrequency;
    [SerializeField] private LayerMask projectilesMask;
    [SerializeField] private int health;

    private float lastUpdateTime;

    public NavMeshAgent agent;

    private void Start()
    {
        // agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 pos, float time)
    {
        if (time - lastUpdateTime > updateFrequency) return;

        lastUpdateTime = time;
        agent.SetDestination(pos);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Projectiles handling
        // TODO should this be here? (NO)

        enemyManager.RegisterHit(other, this);
    }

    public void RegisterHit(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            agent.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            // Destroy(gameObject);
        }
    }

    public AbstractEnemy Enable()
    {
        return this;
    }

    public void Disable()
    {
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
