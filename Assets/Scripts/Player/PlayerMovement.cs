using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRB;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private float easeDuration;
    [SerializeField] private float movementStartTS;

    private Vector2 inputVector;
    private Vector3 velocityVector;

    private Controls controls;


    private void Awake()
    {
        controls = new Controls();
        movementStartTS = 0;
    }

    private void Start()
    {
        DebugTools.ScreenLogger.Instance.AddLine("ease", "test");
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (movementStartTS != 0)
        {
            // var movementDuration = Time.fixedTime - movementStartTS;
            // var moveSpeedEased = EasingFunction.EaseOutQuint(0, easeDuration, movementDuration) * moveSpeed;
            // velocityVector = new Vector3(inputVector.x, 0, inputVector.y) * moveSpeedEased;
            // print(moveSpeedEased);
        }
        else
        {
            // velocityVector = new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed;
        }

        var movementDuration = Time.fixedTime - movementStartTS;
        DebugTools.ScreenLogger.Instance.UpdateLine("ease", movementDuration.ToString());
        // var moveSpeedEased = EasingFunction.EaseOutQuint(0, easeDuration, movementDuration) * moveSpeed;

        velocityVector = new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed;

        if (velocityVector.magnitude > moveSpeed)
        {
            velocityVector = velocityVector.normalized * moveSpeed;
        }

        playerRB.velocity = velocityVector;
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Main.Movement.performed += MovementPerformed;
        controls.Main.Movement.canceled += MovementCanceled;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Main.Movement.performed -= MovementPerformed;
        controls.Main.Movement.canceled -= MovementCanceled;
    }

    private void MovementPerformed(InputAction.CallbackContext context)
    {
        print("Movement performed");
        inputVector = context.ReadValue<Vector2>();

        if (movementStartTS == 0)
        {
            movementStartTS = Time.fixedTime;
        }
    }

    private void MovementCanceled(InputAction.CallbackContext context)
    {
        print("Movement stopped");
        inputVector = Vector2.zero;
        movementStartTS = 0;
    }
}
