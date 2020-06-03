using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameComponents.CurveEditor.Scripts
{
    [Serializable]
    public class Path
    {
        [SerializeField, HideInInspector]
        private List<Vector2> points;

        public Path(Vector2 center)
        {
            points = new List<Vector2>
            {
                // anchor point
                center + Vector2.left, 
                // control point
                center + (Vector2.left + Vector2.up) * 0.5f, 
                // control point
                center + (Vector2.right + Vector2.down) * 0.5f,
                // anchor point
                center + Vector2.right
            };
        }

        public Vector2 this[int i] => points[i];

        public int NumPoints => points.Count;

        public int NumSegments => (points.Count - 4) / 3 + 1;

        /// <summary>
        /// Adds a new segment for path
        /// </summary>
        /// <param name="anchorPos"></param>
        public void AddSegment(Vector2 anchorPos)
        {
            int firstFromEnd = points.Count - 1;
            int secondFromEnd = points.Count - 2;
            
            Vector2 firstControlPoint = points[firstFromEnd] * 2 - points[secondFromEnd];
            points.Add(firstControlPoint);
            
            Vector2 secondControlPoint = (points[firstFromEnd] + anchorPos) / 2;
            points.Add(secondControlPoint);
            
            points.Add(anchorPos);
        }

        public Vector2[] GetPointsInSegment(int i)
        {
            return new []
            {
                points[i * 3],
                points[i * 3 + 1],
                points[i * 3 + 2],
                points[i * 3 + 3]
            };
        }
        
        public void MovePoint(int i, Vector2 pos)
        {
            Vector2 deltaMove = pos - points[i];
            
            points[i] = pos;

            // move control points concurrently with the anchor point
            bool isAnchorPoint = i % 3 == 0;
            if (isAnchorPoint)
            {
                bool notFirstAnchorPoint = i - 1 >= 0;
                if (notFirstAnchorPoint)
                {
                    // move control point
                    points[i - 1] += deltaMove;
                }
                
                bool notLastAnchorPoint = i + 1 < points.Count;
                if (notLastAnchorPoint)
                {
                    // move control point
                    points[i + 1] += deltaMove;
                }
            }
            else
            {
                bool nextPointIsAnchor = (i + 1) % 3 == 0;
                int correspondingControlIndex = nextPointIsAnchor ? i + 2 : i - 2;
                int anchorIndex = nextPointIsAnchor ? i + 1 : i - 1;

                if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count)
                {
                    float dist = (points[anchorIndex] - points[correspondingControlIndex]).magnitude;
                    Vector2 dir = (points[anchorIndex] - pos).normalized;
                    points[correspondingControlIndex] = points[anchorIndex] + dir * dist;
                }
            }
        }
    }
}
