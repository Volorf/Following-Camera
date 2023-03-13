using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private string targetCameraTag = "MainCamera";

    [Space(16)] [Header("Animation")] 
    [SerializeField] private bool animated = true;
    [SerializeField] private float movementSmoothness = 1f;
    [SerializeField] private float rotationSmoothness = 1f;
    
    private Vector3 _movementVelocity = Vector3.zero;
    private Vector3 _rotationVelocity = Vector3.zero;
    
    private Transform _targetCameraTransform;

    private void Start()
    {
        _targetCameraTransform = GameObject.FindGameObjectWithTag(targetCameraTag).transform;
        transform.position = _targetCameraTransform.position;
        transform.forward = _targetCameraTransform.forward;
    }

    private void Update()
    {
        var newPos = Vector3.SmoothDamp(transform.position, _targetCameraTransform.position, ref _movementVelocity,
            movementSmoothness);
        transform.position = newPos;

        var newForw = Vector3.SmoothDamp(transform.forward, _targetCameraTransform.forward, ref _rotationVelocity,
            rotationSmoothness);
        transform.forward = newForw;
    }
}
