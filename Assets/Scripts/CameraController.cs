using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private Transform player;

    [Header("Config")]
    [SerializeField] private float lerpSpeed;

    private Camera _mainCamera;



    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var newPos = player.position + cameraOffset;
        newPos = Vector3.Lerp(_mainCamera.transform.position, newPos, Time.deltaTime * lerpSpeed);
        _mainCamera.transform.position = newPos;

    }
}
