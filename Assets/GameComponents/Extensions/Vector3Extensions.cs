﻿﻿using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
        {
            float newX = x ?? original.x;
            float newY = y ?? original.y;
            float newZ = z ?? original.z;
            return new Vector3(newX, newY, newZ);
        }

        public static bool IsBehind(this Vector3 queried, Vector3 forward)
        {
            return Vector3.Dot(queried, forward) < 0f;
        }
    }
}
