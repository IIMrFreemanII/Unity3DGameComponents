using Extensions;
using UnityEngine;

namespace GameComponents.FPSController
{
    public class MovementController : MonoBehaviour
    {
        private PlayerInputController _playerInputController;
        private FPSController _fpsController;

        [SerializeField] private CapsuleCollider capsuleCollider = null;
        [SerializeField] private Rigidbody rb = null;

        [SerializeField] private LayerMask jumpGroundLayer = default;

        [SerializeField] private float moveSpeed = 300f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] [Range(0f, 1f)] private float groundCheckRadius = 0.05f;

        [SerializeField] [Range(0, 1)] private float counterMovementForce = 0.5f;
        [SerializeField] [Range(0, 1)] private float counterMovementForceInAir = 0.025f;

        private bool IsGrounded
        {
            get
            {
                Vector3 bottomCenterPoint = new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y,
                    capsuleCollider.bounds.center.z);

                float decreasedRadius = capsuleCollider.bounds.size.x / 2 * groundCheckRadius;
                return Physics.CheckCapsule(capsuleCollider.bounds.center, bottomCenterPoint, decreasedRadius,
                    jumpGroundLayer);
            }
        }

        public void Initialize()
        {
            _fpsController = GetComponent<FPSController>();

            _playerInputController = GetComponent<PlayerInputController>();

            capsuleCollider = capsuleCollider != null
                ? capsuleCollider
                : transform.GetChild(0).GetComponent<CapsuleCollider>();
            rb = rb != null ? rb : GetComponent<Rigidbody>();

            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ |
                             RigidbodyConstraints.FreezeRotationY;

            if (jumpGroundLayer == gameObject.layer)
            {
                Debug.LogError("Player SortingLayer must be different from Ground Sorting Layer!");
            }
        }

        public void UpdateMovement()
        {
            HandleMovement();
            HandleRotation();
            HandleJump();
        }

        private void HandleRotation()
        {
            float deltaMouseX = _playerInputController.DeltaMouseX;
            float rotationSensitivity = _fpsController.rotationSensitivity;
            bool isRotating = Mathf.Abs(deltaMouseX) > 0;

            if (isRotating) transform.Rotate(new Vector3(0, deltaMouseX * rotationSensitivity, 0));
        }

        private void HandleMovement()
        {
            var (
                    horizontal,
                    vertical
                    )
                =
                (
                    _playerInputController.Horizontal,
                    _playerInputController.Vertical
                );

            float absHorizontal = Mathf.Abs(horizontal);
            float absVertical = Mathf.Abs(vertical);
            bool inAir = rb.velocity.y != 0;

            bool isHorizontalMove = absHorizontal > 0;
            bool isVerticalMove = absVertical > 0;
            bool isMoving = isHorizontalMove || isVerticalMove;

            if (isMoving)
            {
                // desired move direction relative to the current rotation
                Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;

                // to get direction relative to the current rotation:
                // 1. transform.rotation * moveDir;
                // or
                // 2. transform.TransformDirection(moveDir);
                Vector3 movement = (transform.rotation * moveDir) * (moveSpeed * Time.fixedDeltaTime);
                rb.velocity = movement.With(y: rb.velocity.y);
            }

            void CounterMove()
            {
                if (!isMoving && IsGrounded)
                {
                    rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, counterMovementForce).With(y: rb.velocity.y);
                }

                if (inAir)
                {
                    rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, counterMovementForceInAir)
                        .With(y: rb.velocity.y);
                }
            }

            CounterMove();
        }

        private void HandleJump()
        {
            float jump = _playerInputController.Jump;

            if (IsGrounded && jump > 0)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}