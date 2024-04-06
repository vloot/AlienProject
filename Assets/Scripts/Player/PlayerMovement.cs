using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRB;

    [Header("Movement")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed;

    [Header("Dash")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashMovementThreshold = 0.1f;
    [SerializeField] private EasingFunction.EaseType easeType;
    private Vector3 _dashDirection;
    private bool _isDashing;
    private float _dashTimeStart;

    private Vector2 inputVector;
    private Vector3 movementVector;

    private Controls controls;


    private void Awake()
    {
        controls = new Controls();
        // movementStartTS = 0;
    }

    private void Start()
    {
    }

    private void Update()
    {
        movementVector = GetVectorFromInput() * moveSpeed;

        if (movementVector.magnitude > moveSpeed)
        {
            movementVector = movementVector.normalized * moveSpeed;
        }

        var currentDashDuration = Time.time - _dashTimeStart;

        if (_isDashing && currentDashDuration <= dashDuration)
        {
            var dashPercentage = currentDashDuration / dashDuration;
            var newDashForce = EasingFunction.GetEasingFunction(easeType)(0, dashForce, dashPercentage);
            movementVector = _dashDirection * newDashForce;
        }
        else if (_isDashing)
        {
            _isDashing = false;
        }

        movementVector.y = 0;
        controller.Move(movementVector * Time.deltaTime);
        // playerRB.velocity = velocityVector;
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Main.Movement.performed += MovementPerformed;
        controls.Main.Movement.canceled += MovementCanceled;
        controls.Main.Dash.performed += DashPerformed;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Main.Movement.performed -= MovementPerformed;
        controls.Main.Movement.canceled -= MovementCanceled;
        controls.Main.Dash.performed -= MovementCanceled;
    }

    private void MovementPerformed(InputAction.CallbackContext context)
    {
        ConsoleLogger.Log("Movement performed", "InputSystem");
        inputVector = context.ReadValue<Vector2>();
    }

    private void MovementCanceled(InputAction.CallbackContext context)
    {
        ConsoleLogger.Log("Movement stopped", "InputSystem");
        inputVector = Vector2.zero;
    }

    private void DashPerformed(InputAction.CallbackContext context)
    {
        if (_isDashing) return;
        else if (Time.time - _dashTimeStart + dashDuration < dashCooldown) return;

        ConsoleLogger.Log("Dash performed");
        _dashTimeStart = Time.time;
        _isDashing = true;


        if (inputVector.magnitude >= dashMovementThreshold)
        {
            _dashDirection = GetVectorFromInput();
        }
        else
        {
            _dashDirection = transform.forward;
        }
    }

    private Vector3 GetVectorFromInput()
    {
        return new Vector3(inputVector.x, 0, inputVector.y);
    }
}
