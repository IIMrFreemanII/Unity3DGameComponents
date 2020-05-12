using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Rigidbody projectileRb;
    [SerializeField] private Transform targetTransform;

    [SerializeField] private float hDisplacement = 25f;
    [SerializeField] private float gravity = -18f;

    [SerializeField] private bool debugPath;
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
            DrawPath();
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
        Vector3 displacementXZ = new Vector3(targetTransform.position.x - projectileRb.position.x, 0, targetTransform.position.z - projectileRb.position.z);

        if (hDisplacement < displacementY)
        {
            Debug.LogError("hDisplacement must be greater than yDisplacement between launchPosY and targetPosY");
            return new LaunchData();
        }

        float time = Mathf.Sqrt(-2 * hDisplacement / gravity) +
                     Mathf.Sqrt(2 * (displacementY - hDisplacement) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * hDisplacement);
        Vector3 velocityXZ = displacementXZ / time;

        Vector3 initialVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);
        
        return new LaunchData(initialVelocity, time);
    }

    private void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = projectileRb.position;

        int resolution = drawPathResolution;
        for (int i = 1; i < resolution; i++)
        {
            float simulationTime = i / (float) resolution * launchData.timeToTarget;
            
            Vector3 displacement = launchData.initialVelocity * simulationTime + (Vector3.up * gravity) *
                (simulationTime * simulationTime) / 2f;
            Vector3 drawPoint = projectileRb.position + displacement;
            
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            
            previousDrawPoint = drawPoint;
        }
    }

    struct LaunchData
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
