using UnityEngine;

namespace GameComponents.FPSController
{
    public class CameraController : MonoBehaviour
    {
        private PlayerInputController _playerInputController;
        private Transform _cameraTransform;
        private TargetCameraTransform _targetCameraTransform;
        private FPSController _fpsController;

        private float _currentXRotation;

        [SerializeField] private float clamXRotationMin = -45f;
        [SerializeField] private float clamXRotationMax = 75f;

        public void Initialize()
        {
            _fpsController = GetComponent<FPSController>();
            _playerInputController = GetComponent<PlayerInputController>();

            _cameraTransform = _fpsController.camera.transform;
            _targetCameraTransform = _fpsController.targetCameraTransform;
            
            _cameraTransform.SetParent(_targetCameraTransform.transform);
            _cameraTransform.localPosition = Vector3.zero;
            _cameraTransform.localEulerAngles = Vector3.zero;
        }

        private void HandleRotation()
        {
            var ( deltaMouseX, deltaMouseY) = (_playerInputController.DeltaMouseX, _playerInputController.DeltaMouseY);
            float rotationSensitivity = _fpsController.rotationSensitivity;

            bool isRotating = Mathf.Abs(deltaMouseX) > 0 || Mathf.Abs(deltaMouseY) > 0;
            if (!isRotating) return;
            
            _currentXRotation -= deltaMouseY * rotationSensitivity;
            _currentXRotation = Mathf.Clamp(_currentXRotation, clamXRotationMin, clamXRotationMax);

            _cameraTransform.localRotation = Quaternion.Euler(_currentXRotation, 0, 0);
        }

        public void UpdateCamera()
        {
            HandleRotation();
        }
    }
}