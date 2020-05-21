﻿﻿using System;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static bool HandleComponent<T>(this GameObject gameObject, Action<T> handler)
        {
            if (gameObject.TryGetComponent(out T component))
            {
                handler?.Invoke(component);
                return true;
            }

            return false;
        }
    }
}