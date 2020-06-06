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

        [SerializeField, HideInInspector] 
        private bool isClosed;
        
        [SerializeField, HideInInspector] 
        private bool autoSetControlPoints;
        
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

        public bool IsClosed
        {
            get => isClosed;
            set
            {
                if (isClosed != value)
                {
                    isClosed = value;

                    if (isClosed)
                    {
                        int firstFromEnd = points.Count - 1;
                        int secondFromEnd = points.Count - 2;
            
                        Vector2 firstControlPoint = points[firstFromEnd] * 2 - points[secondFromEnd];
                        points.Add(firstControlPoint);

                        int firstPoint = 0;
                        int secondPoint = 1;
                        points.Add(points[firstPoint] * 2 - points[secondPoint]);

                        if (autoSetControlPoints)
                        {
                            AutoSetAnchorControlPoints(0);
                            AutoSetAnchorControlPoints(points.Count - 3);
                        }
                    }
                    else
                    {
                        points.RemoveRange(points.Count - 2, 2);

                        if (autoSetControlPoints)
                        {
                            AutoSetStartAndEndControls();
                        }
                    }
                }
            }
        }

        public bool AutoSetControlPoints
        {
            get => autoSetControlPoints;
            set
            {
                if (autoSetControlPoints != value)
                {
                    autoSetControlPoints = value;
                    if (autoSetControlPoints)
                    {
                        AutoSetAllControlPoints();
                    }
                }
            }
        }

        public int NumPoints => points.Count;

        public int NumSegments => points.Count / 3;

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

            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(points.Count - 1);
            }
        }

        public void SplitSegment(Vector2 anchorPos, int segmentIndex)
        {
            points.InsertRange(segmentIndex * 3 + 2, new Vector2[] {Vector2.zero, anchorPos, Vector2.zero});

            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
            }
            else
            {
                AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
            }
        }

        public void DeleteSegment(int anchorIndex)
        {
            if (NumSegments > 2 || !isClosed && NumSegments > 1)
            { 
                if (anchorIndex == 0)
                {
                    if (isClosed)
                    {
                        points[points.Count - 1] = points[2];
                        points.RemoveRange(0, 3);
                    }
                } else if (anchorIndex == points.Count - 1 && !isClosed)
                {
                    points.RemoveRange(anchorIndex - 2, 3);
                }
                else
                {
                    points.RemoveRange(anchorIndex - 1, 3);
                }
            }
        }

        public Vector2[] GetPointsInSegment(int i)
        {
            return new []
            {
                points[i * 3],
                points[i * 3 + 1],
                points[i * 3 + 2],
                points[LoopIndex(i * 3 + 3)]
            };
        }
        
        public void MovePoint(int i, Vector2 pos)
        {
            Vector2 deltaMove = pos - points[i];
            bool isAnchorPoint = i % 3 == 0;

            if (isAnchorPoint || !autoSetControlPoints)
            {
                points[i] = pos;

                if (autoSetControlPoints)
                {
                    AutoSetAllAffectedControlPoints(i);
                }
                else
                {
                    // move control points concurrently with the anchor point
                    // bool isAnchorPoint = i % 3 == 0;
                    if (isAnchorPoint)
                    {
                        bool notFirstAnchorPoint = i - 1 >= 0 || isClosed;
                        if (notFirstAnchorPoint)
                        {
                            // move control point
                            points[LoopIndex(i - 1)] += deltaMove;
                        }
                
                        bool notLastAnchorPoint = i + 1 < points.Count || isClosed;
                        if (notLastAnchorPoint)
                        {
                            // move control point
                            points[LoopIndex(i + 1)] += deltaMove;
                        }
                    }
                    else
                    {
                        bool nextPointIsAnchor = (i + 1) % 3 == 0;
                        int correspondingControlIndex = nextPointIsAnchor ? i + 2 : i - 2;
                        int anchorIndex = nextPointIsAnchor ? i + 1 : i - 1;

                        if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                        {
                            float dist = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                            Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                            points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * dist;
                        }
                    }
                }
            }
            
        }

        private int LoopIndex(int i)
        {
            return (i + points.Count) % points.Count;
        }

        private void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
        {
            for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
            {
                if (i >= 0 && i < points.Count || isClosed)
                {
                    AutoSetAnchorControlPoints(LoopIndex(i));
                }
            }
            
            AutoSetStartAndEndControls();
        }

        private void AutoSetAllControlPoints()
        {
            for (int i = 0; i < points.Count; i += 3)
            {
                AutoSetAnchorControlPoints(i);
                
            }

            AutoSetStartAndEndControls();
        }

        private void AutoSetAnchorControlPoints(int anchorIndex)
        {
            Vector2 anchorPos = points[anchorIndex];
            Vector2 dir = Vector2.zero;
            float[] neighbourDistances = new float[2];

            if (anchorIndex - 3 >= 0 || isClosed)
            {
                Vector2 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
                dir += offset.normalized;
                neighbourDistances[0] = offset.magnitude;
            }

            if (anchorIndex + 3 >= 0 || isClosed)
            {
                Vector2 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
                dir -= offset.normalized;
                neighbourDistances[1] = -offset.magnitude;
            }
            
            dir.Normalize();

            for (int i = 0; i < 2; i++)
            {
                int controlIndex = anchorIndex + i * 2 - 1;
                if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
                {
                    points[LoopIndex(controlIndex)] = anchorPos + dir * neighbourDistances[i] * 0.5f;
                }
            }
        }

        private void AutoSetStartAndEndControls()
        {
            if (!isClosed)
            {
                int firstFromEnd = points.Count - 1;
                int secondFromEnd = points.Count - 2;
                int thirdFromEnd = points.Count - 3;
                
                points[1] = (points[0] + points[2]) * 0.5f;
                points[secondFromEnd] = (points[firstFromEnd] + points[thirdFromEnd]) * 0.5f;
            }
        }
    }
}
