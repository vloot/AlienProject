using UnityEngine;

[System.Serializable]
public abstract class AbstractProjectile : MonoBehaviour, IPoolObject<AbstractProjectile>
{
    [SerializeField] protected Rigidbody rgBody;

    // parameters
    public bool isFired;
    public Vector3 direction;
    public float creationTime;

    public ProjectileManager manager;

    public void Launch(Vector3 direction, Quaternion rotation, float time)
    {
        transform.rotation = rotation;
        creationTime = time;
        this.direction = direction;
        isFired = true;
    }

    public abstract void UpdateProjectile(float deltaTime);

    public AbstractProjectile Enable()
    {
        gameObject.SetActive(true); // TODO disabling mesh is cheaper
        rgBody.isKinematic = false;
        return this;
    }

    public void Disable()
    {
        isFired = false;

        rgBody.isKinematic = false;
        rgBody.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rgBody.isKinematic = true;

        gameObject.SetActive(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void OnCollisionEnter(Collision other)
    {
        manager.OnCollision(this, other);
    }
}
