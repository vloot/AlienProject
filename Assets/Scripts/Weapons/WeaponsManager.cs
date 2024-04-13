using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private AbstractWeapon currentWeapon;

    public AbstractWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void RegisterHit(GameObject enemy)
    {
        enemyManager.RegisterHit(enemy);
    }
}
