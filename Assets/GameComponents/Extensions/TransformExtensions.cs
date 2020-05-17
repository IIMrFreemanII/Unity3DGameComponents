﻿﻿using System;
  using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static Vector3 DirectionTo(this Transform transform, Vector3 destination)
        {
            return Vector3.Normalize(destination - transform.position);
        }
        
        public static void HandleComponent<T>(this Transform transform, Action<T> handler)
        {
            if (transform.TryGetComponent(out T component))
            {
                handler?.Invoke(component);
            }
        }
    }
}