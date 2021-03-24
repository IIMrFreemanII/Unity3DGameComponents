using System;
using UnityEngine;

// Drag this on your player
namespace GameComponents.FPSController
{
    [RequireComponent(typeof(CameraController))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(PlayerInputController))]
    public class FPSController : MonoBehaviour
    {
        public new Camera camera;
        public TargetCameraTransform targetCameraTransform = null;

        private CameraController _cameraController;
        private MovementController _movementController;
        private PlayerInputController _playerInputController;

        public bool lockCursor;
        
        [Range(0, 2)] public float rotationSensitivity = 1f;

        private void Awake()
        {
            camera = camera != null ? camera : Camera.main;

            _cameraController = GetComponent<CameraController>();
            _movementController = GetComponent<MovementController>();
            _playerInputController = GetComponent<PlayerInputController>();
            
            _cameraController.Initialize();
            _movementController.Initialize();
        }

        private void Start()
        {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void LateUpdate()
        {
            _cameraController.UpdateCamera();
        }

        private void FixedUpdate()
        {
            _movementController.HandleMovement();
            _movementController.HandleJump();
        }

        private void Update()
        {
            _playerInputController.UpdateInput();
            _movementController.HandleRotation();
        }
    }
}