using UnityEngine;

namespace GameComponents.Utils.CameraScripts
{
    public class Camera3DMovement : MonoBehaviour
    {
        public float moveSpeed = 10f;

        private float horizontalInput;
        private float verticalInput;
        private float mouseX;
        private float mouseY;

        private float currentXRot;
        private float currentYRot;

        private void Update ()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            Vector3 moveAmount = new Vector3(horizontalInput, 0, verticalInput);

            if (Input.GetKey(KeyCode.Q))
            {
                moveAmount += Vector3.up;
            }
            
            if (Input.GetKey(KeyCode.E))
            {
                moveAmount += Vector3.down;
            }

            moveAmount *= (moveSpeed * Time.deltaTime);
            
            transform.Translate(moveAmount);
        }

        private void HandleRotation()
        {
            if (Input.GetMouseButton(1))
            {
                currentXRot -= mouseY;
                currentYRot += mouseX;

                Vector3 rotationAmount = new Vector3(currentXRot, currentYRot, 0);
            
                transform.rotation = Quaternion.Euler(rotationAmount);
            }
        }
    }
}