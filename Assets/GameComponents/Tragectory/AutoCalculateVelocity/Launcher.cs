using UnityEngine;

namespace AutoCalculateVelocity
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] private Rigidbody projectileRb = null;
        [SerializeField] private Transform targetTransform = null;

        [SerializeField] private float hDisplacement = 25f;
        [SerializeField] private float gravity = -18f;

        [SerializeField] private bool debugPath = true;
        [SerializeField] private int drawPathResolution = 40;

        private void Start()
        {
            projectileRb.isKinematic = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Launch();
            }

            if (debugPath)
            {
                LaunchData launchData = CalculateLaunchData();
                DrawPath(projectileRb.position, launchData.initialVelocity, launchData.timeToTarget, gravity, drawPathResolution);
            }
        }

        private void Launch()
        {
            Physics.gravity = Vector3.up * gravity;
            projectileRb.isKinematic = false;

            projectileRb.velocity = CalculateLaunchData().initialVelocity;
        }

        private LaunchData CalculateLaunchData()
        {
            float displacementY = targetTransform.position.y - projectileRb.position.y;
            Vector3 displacementXZ = new Vector3(targetTransform.position.x - projectileRb.position.x, 0,
                targetTransform.position.z - projectileRb.position.z);

            if (hDisplacement < displacementY)
            {
                Debug.LogError("hDisplacement must be greater than yDisplacement between launchPosY and targetPosY");
                return new LaunchData();
            }

            float timeToTop = Mathf.Sqrt(-2 * hDisplacement / gravity);
            float timeFromTopToTarget = Mathf.Sqrt(2 * (displacementY - hDisplacement) / gravity);
            float time = timeToTop + timeFromTopToTarget;

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * hDisplacement);
            Vector3 velocityXZ = displacementXZ / time;

            Vector3 initialVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);

            return new LaunchData(initialVelocity, time);
        }

        private void DrawPath(Vector3 currentPosition, Vector3 initialVelocity, float timeToTarget, float gravity, int pathResolution)
        {
            Vector3 previousDrawPoint = currentPosition;

            for (int i = 1; i < pathResolution; i++)
            {
                float simulationTime = i / (float) pathResolution * timeToTarget;

                Vector3 displacement = initialVelocity * simulationTime + (Vector3.up * gravity) *
                    (simulationTime * simulationTime) / 2f;
                Vector3 drawPoint = currentPosition + displacement;

                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);

                previousDrawPoint = drawPoint;
            }
        }

        readonly struct LaunchData
        {
            public readonly Vector3 initialVelocity;
            public readonly float timeToTarget;

            public LaunchData(Vector3 initialVelocity, float timeToTarget)
            {
                this.timeToTarget = timeToTarget;
                this.initialVelocity = initialVelocity;
            }
        }
    }
}