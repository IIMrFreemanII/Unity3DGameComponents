using UnityEngine;

namespace GameComponents.FPSController
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private float deltaMouseX;
        public float DeltaMouseX
        {
            get => deltaMouseX;
            private set => deltaMouseX = value;
        }

        [SerializeField] private float deltaMouseY;
        public float DeltaMouseY
        {
            get => deltaMouseY;
            private set => deltaMouseY = value;
        }

        [SerializeField] private float horizontal;
        public float Horizontal
        {
            get => horizontal;
            private set => horizontal = value;
        }
        
        [SerializeField] private float vertical;
        public float Vertical
        {
            get => vertical;
            private set => vertical = value;
        }
        
        [SerializeField] private float jump;
        public float Jump
        {
            get => jump;
            private set => jump = value;
        }

        public void UpdateInput()
        {
            DeltaMouseX = Input.GetAxis("Mouse X");
            DeltaMouseY = Input.GetAxis("Mouse Y");

            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");
            Jump = Input.GetAxis("Jump");
        }
    }
    
}