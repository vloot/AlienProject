using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject<Bullet>
{
    public bool _isFired;
    public Vector3 direction;
    public float creationTime;

    public void Disable()
    {
        _isFired = false;
        gameObject.SetActive(false);
    }

    public Bullet Enable()
    {
        gameObject.SetActive(true);
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

    public void Fire(Vector3 direction, Quaternion rotation, float time)
    {
        transform.rotation = rotation;
        creationTime = time;
        this.direction = direction;
        _isFired = true;
    }
}

