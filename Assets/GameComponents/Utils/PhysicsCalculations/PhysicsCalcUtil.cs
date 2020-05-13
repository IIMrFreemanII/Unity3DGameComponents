using UnityEngine;

namespace GameComponents.Utils.PhysicsCalculations
{
    public static class PhysicsCalcUtil
    {
        public readonly struct LaunchData
        {
            public readonly Vector3 initialVelocity;
            public readonly float timeToTarget;

            public LaunchData(Vector3 initialVelocity, float timeToTarget)
            {
                this.timeToTarget = timeToTarget;
                this.initialVelocity = initialVelocity;
            }
        }
        
        public static LaunchData CalcInitVelocity(Vector3 startPos, Vector3 targetPos, float maxHeight, float gravity)
        {
            float displacementY = targetPos.y - startPos.y;
            Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0,
                targetPos.z - startPos.z);

            if (maxHeight < displacementY)
            {
                Debug.LogError("maxHeight must be greater than yDisplacement between startPosY and targetPosY");
                return new LaunchData();
            }

            float timeToMaxHeight = Mathf.Sqrt(-2 * maxHeight / gravity);
            float timeFromMaxHeightToTarget = Mathf.Sqrt(2 * (displacementY - maxHeight) / gravity);
            float time = timeToMaxHeight + timeFromMaxHeightToTarget;

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxHeight);
            Vector3 velocityXZ = displacementXZ / time;

            Vector3 initialVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);

            return new LaunchData(initialVelocity, time);
        }
    }
}