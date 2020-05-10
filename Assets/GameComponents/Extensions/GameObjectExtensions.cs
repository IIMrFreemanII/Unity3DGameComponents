﻿﻿using System;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static void HandleComponent<T>(this GameObject gameObject, Action<T> handler)
        {
            if (gameObject.TryGetComponent(out T component))
            {
                handler?.Invoke(component);
            }
        }
    }
}