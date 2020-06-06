using GameComponents.CurveEditor.Scripts;
using UnityEngine;

namespace GameComponents.CurveEditor.Examples
{
    public class PathPlacer : MonoBehaviour
    {
        public float spacing = 0.1f;
        public float resolutions = 1f;

        private PathCreator pathCreator;

        private void Start()
        {
            pathCreator = FindObjectOfType<PathCreator>();
            Vector2[] points = pathCreator.path.CalculateEvenlySpacedPoints(spacing, resolutions);
            
            foreach (Vector2 point in points)
            {
                GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gameObject.transform.position = point;
                gameObject.transform.localScale = Vector3.one * spacing * 0.5f;
            }
        }
    }
}