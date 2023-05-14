using UnityEngine;

namespace Volorf.FollowingCamera
{
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private string targetCameraTag = "MainCamera";
        [SerializeField] private string targetFocusTag = "TargetFocus";
        [SerializeField] private bool isTargetFocisMode = false;
        [SerializeField] private float offset = 0f;

        [Space(16)] [Header("Animation")]
        [SerializeField] private bool animated = true;
        [SerializeField] private float movementSmoothness = 1f;
        [SerializeField] private float rotationSmoothness = 1f;

        [Space(16)] [Header("Copy Target Camera Parameters")]
        [SerializeField] private bool copyFOV = true;
        [SerializeField] private bool copyClearFlag = true;
        [SerializeField] private bool copyBackground = true;
        
        
        private Vector3 _movementVelocity = Vector3.zero;
        private Vector3 _rotationVelocity = Vector3.zero;

        private Transform _targetCameraTransform;
        private Camera _targetCamera;
        private Camera _thisCamera;
        private Transform _targetFocusTransform;

        private void Start()
        {
            GameObject cameraGO = GameObject.FindGameObjectWithTag(targetCameraTag);
            _targetFocusTransform = GameObject.FindGameObjectWithTag(targetFocusTag).transform;

            _targetCameraTransform = cameraGO.transform;
            _targetCamera = cameraGO.GetComponent<Camera>();
            transform.position = _targetCameraTransform.position;
            transform.forward = _targetCameraTransform.forward;
            
            // Copy target camera parameters
            _thisCamera = GetComponent<Camera>();
            if (copyFOV) _thisCamera.fieldOfView = _targetCamera.fieldOfView;
            if (copyClearFlag) _thisCamera.clearFlags = _targetCamera.clearFlags;
            if (copyBackground) _thisCamera.backgroundColor = _targetCamera.backgroundColor;
        }

        private void Update()
        {
            if (isTargetFocisMode)
            {
                var towardsTargetVec = (_targetFocusTransform.position - _targetCameraTransform.position).normalized;
                Vector3 targetPos = _targetCameraTransform.position + towardsTargetVec * offset;
                var newPos = Vector3.SmoothDamp(transform.position, targetPos, ref _movementVelocity,
                    movementSmoothness);
                transform.position = newPos;
            }
            else
            {
                Vector3 targetPos = _targetCameraTransform.forward * offset + _targetCameraTransform.position;
                var newPos = Vector3.SmoothDamp(transform.position, targetPos, ref _movementVelocity,
                    movementSmoothness);
                transform.position = newPos;
            }

            if (isTargetFocisMode)
            {
                var towardsTargetVec = (_targetFocusTransform.position - _targetCameraTransform.position).normalized;
                var newForw = Vector3.SmoothDamp(transform.forward, towardsTargetVec, ref _rotationVelocity,
                    rotationSmoothness);
                transform.forward = newForw;
            }
            else
            {
                var newForw = Vector3.SmoothDamp(transform.forward, _targetCameraTransform.forward, ref _rotationVelocity,
                    rotationSmoothness);
                transform.forward = newForw;
            }
            
            
        }
    }
}