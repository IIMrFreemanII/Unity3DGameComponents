using Extensions;
using UnityEngine;

namespace TopDownArcher
{
    public class ArcherTopDownController : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float rotationSpeed = 5f;

        public Vector3 targetPosition;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            RotateToMousePosition();
        }

        private void RotateToMousePosition()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Vector3 targetLookDirection;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetLookDirection = hit.point - transform.position;
                targetPosition = hit.point;
            }
            else
            {
                targetLookDirection = ray.direction;
            }

            targetLookDirection = targetLookDirection.With(y: 0);

            Quaternion targetRotation = Quaternion.LookRotation(targetLookDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}