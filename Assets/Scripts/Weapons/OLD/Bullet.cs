using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject<Bullet>
{
    [SerializeField] private Rigidbody rgBody;

    [Header("Variables")]
    public bool isFired;
    public Vector3 direction;
    public float creationTime;

    public delegate void OnCollisionDelegate(Bullet bullet, Collision other);
    public static OnCollisionDelegate OnCollision;

    public void Disable()
    {
        isFired = false;
        rgBody.isKinematic = false;
        rgBody.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rgBody.isKinematic = true;
        gameObject.SetActive(false);
    }

    public Bullet Enable()
    {
        gameObject.SetActive(true);
        rgBody.isKinematic = false;
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
        isFired = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        OnCollision?.Invoke(this, other);
    }
}

