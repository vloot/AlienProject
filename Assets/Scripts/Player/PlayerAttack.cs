using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PlayerRotation playerRotation;
    [SerializeField] private AbstractWeapon weapon;

    private Controls controls;
    private bool _isAttacking;

    private void Awake()
    {
        controls = new Controls();

    }

    private void Update()
    {
        if (!_isAttacking) return;

        weapon.Use(spawnPoint.position, playerTransform.forward, playerTransform.rotation);
    }

    private void OnEnable()
    {
        _isAttacking = false;
        controls.Enable();
        controls.Main.AttackPrimary.performed += AttackPerformed;
        controls.Main.AttackPrimary.canceled += AttackCanceled;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Main.AttackPrimary.performed -= AttackPerformed;
        controls.Main.AttackPrimary.canceled -= AttackCanceled;
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        ConsoleLogger.Log("_isAttacking = true", "InputSystem");
        _isAttacking = true;
    }

    private void AttackCanceled(InputAction.CallbackContext context)
    {
        ConsoleLogger.Log("_isAttacking = false", "InputSystem");
        _isAttacking = false;
    }
}
