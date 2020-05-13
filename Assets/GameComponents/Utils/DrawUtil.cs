using UnityEngine;

namespace GameComponents.Utils
{
    public static class DrawUtil
    {
        public static void DrawParabolicPath(Vector3 currentPosition, Vector3 initialVelocity, float timeToTarget, float gravity, int pathResolution)
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
    }
}