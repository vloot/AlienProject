using UnityEngine;
using UnityEngine.AI;

public class AbstractEnemy : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [Header("Stats")]
    [SerializeField] private float maxDelta;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float updateFrequency;

    [SerializeField] private LayerMask projectilesMask;
    private float lastUpdateTime;


    public NavMeshAgent agent;

    private void Start()
    {
        // agent = GetComponent<NavMeshAgent>();
        // agent.SetDestination(playerTransform.position);
    }

    public void SetDestination(Vector3 pos, float time)
    {
        if (time - lastUpdateTime > updateFrequency) return;

        lastUpdateTime = time;
        agent.SetDestination(pos);
    }

    private void OnCollisionEnter(Collision other)
    {
        print("collided");

        if (projectilesMask.Contains(other.gameObject.layer))
        {
            gameObject.SetActive(false);
        }
    }
}
