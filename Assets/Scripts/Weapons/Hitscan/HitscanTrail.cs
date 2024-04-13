using UnityEngine;

public class HitscanTrail : MonoBehaviour, IPoolObject<HitscanTrail>
{
    public TrailRenderer trailRenderer;

    public void Disable()
    {
        trailRenderer.enabled = false;
    }

    public HitscanTrail Enable()
    {
        trailRenderer.enabled = true;
        return this;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
