using UnityEngine;

public class BulletProjectile : AbstractProjectile
{
    public override void UpdateProjectile(float deltaTime)
    {
        // rgBody.AddForce(direction * deltaTime);
        transform.position += direction * deltaTime;
    }
}
