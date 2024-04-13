using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HitscanVisuals : MonoBehaviour
{

    [Header("Parameters")]
    [SerializeField] private float trailDuration;

    [Header("Object pool")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private int initSize;
    [SerializeField] private int increaseFactor;
    private ObjectPool<HitscanTrail> _trailsPool;

    private void Start()
    {
        _trailsPool = new ObjectPool<HitscanTrail>(initSize, prefab, spawnParent, increaseFactor);
    }

    public HitscanTrail SpawnTrail(Vector3 pos, Quaternion rotation, Vector3 targetPos)
    {
        var trail = _trailsPool.Spawn(pos, false);
        trail.GetTransform().rotation = rotation;

        StartCoroutine(SpawnTrail(trail, pos, targetPos));

        return trail;
    }

    private IEnumerator SpawnTrail(HitscanTrail trail, Vector3 pos, Vector3 targetPos)
    {
        float time = 0;

        trail.GetTransform().position = pos;
        trail.Enable();

        while (time < trailDuration)
        {
            trail.GetTransform().position = Vector3.Lerp(pos, targetPos, time);
            time += Time.deltaTime / trail.trailRenderer.time;

            yield return null;
        }

        _trailsPool.Despawn(trail);
    }
}
