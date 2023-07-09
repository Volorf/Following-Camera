using UnityEngine;

namespace Volorf.FollowingCamera
{
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private string targetCameraTag = "MainCamera";
        [SerializeField] private string targetFocusTag = "TargetFocus";
        [SerializeField] private bool isTargetFocusMode = false;
        [SerializeField] private float offsetAlongCamera = 0f;

        [Header("Target Focus Offsets")] 
        [SerializeField] private float tfOffsetX;
        [SerializeField] private float tfOffsetY;
        [SerializeField] private float tfOffsetZ;

        [Header("Camera Offsets")] 
        [SerializeField] private float camOffsetX;
        [SerializeField] private float camOffsetY;
        [SerializeField] private float camOffsetZ;
        
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
            Vector3 cameraOffset = new Vector3(camOffsetX, camOffsetY, camOffsetZ);
            Vector3 newTFPos = new Vector3();
            
            if (isTargetFocusMode)
            {
                Vector3 offsets = new Vector3();
                offsets += _targetFocusTransform.right * tfOffsetX;
                offsets += _targetFocusTransform.forward * tfOffsetZ;
                offsets += _targetFocusTransform.up * tfOffsetY;
                
                newTFPos = _targetFocusTransform.position + offsets;
                
                var towardsTargetVec = (newTFPos - _targetCameraTransform.position).normalized;
                Vector3 targetPos = _targetCameraTransform.position + towardsTargetVec * offsetAlongCamera;
                var newPos = Vector3.SmoothDamp(transform.position, targetPos, ref _movementVelocity,
                    movementSmoothness);
                transform.position = newPos + cameraOffset;
            }
            else
            {
                Vector3 targetPos = _targetCameraTransform.forward * offsetAlongCamera + _targetCameraTransform.position;
                var newPos = Vector3.SmoothDamp(transform.position, targetPos, ref _movementVelocity,
                    movementSmoothness);
                transform.position = newPos + cameraOffset;
            }

            if (isTargetFocusMode)
            {
                var towardsTargetVec = (newTFPos - _targetCameraTransform.position).normalized;
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