using UnityEngine;

public class HitscanWeapon : AbstractWeapon
{
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask hitLayerMask;
    [SerializeField] private HitscanVisuals hitscanVisuals;
    [SerializeField] private int framesHitDelay;

    public override void Use(Vector3 position, Vector3 direction, Quaternion rotation)
    {
        var time = Time.time;
        if (!CanFire(time)) return;
        _timeSinceLastBullet = time;


        direction.x += Random.Range(weaponStats.spread.x, weaponStats.spread.y);
        direction.z += Random.Range(weaponStats.spread.x, weaponStats.spread.y);

        Debug.DrawRay(position, direction * maxDistance, Color.red);

        if (Physics.Raycast(position, direction, out RaycastHit hit, maxDistance, hitLayerMask))
        {
            hitscanVisuals.SpawnTrail(position, rotation, hit.point);
            // hit.collider.gameObject.GetComponent<
            weaponsManager.RegisterHit(hit.collider.gameObject);
            // TODO add frame delay
        }
        else
        {
            // TODO finish no hit, Spawn trail in front of player
        }

    }
}
