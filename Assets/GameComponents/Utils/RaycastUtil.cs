using UnityEngine;

namespace GameComponents.Utils
{
    public static class RayCastUtil
    {
        /// <summary>
        /// if hit some collider return the hit position otherwise return the normalized direction of the ray movement.
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        public static Vector3 MouseToHitPos(Camera cam, Vector3 mousePos)
        {
            Ray ray = cam.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }

            return ray.direction;
        }
    }
}