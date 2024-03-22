using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField] private Transform cursorGroundIndicator;
    [SerializeField] private Transform player;

    private Vector3 playerCursorTargetPos;

    public Vector3 PlayerCursorTargetPos { get => playerCursorTargetPos; }

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        FaceCursor();
    }

    private void FaceCursor()
    {
        var cursorPosition = GetCursorPosition();
        if (cursorPosition == Vector3.zero) return;

        var cursorPositionGround = cursorPosition;
        cursorPositionGround.y = 0;
        cursorGroundIndicator.position = cursorPositionGround;

        playerCursorTargetPos = cursorPosition;
        playerCursorTargetPos.y = player.position.y;

        player.LookAt(playerCursorTargetPos);
    }

    private Vector3 GetCursorPosition()
    {
        var pos = Input.mousePosition;
        Ray ray = _mainCamera.ScreenPointToRay(pos);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, terrainLayerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
