﻿using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int lineSegments = 50;

    public bool Enabled
    {
        get => lineRenderer.enabled;
        set => lineRenderer.enabled = value;
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[lineSegments];
        lineRenderer.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * Time.fixedDeltaTime;

            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;

            if (points[i].y < 0)
            {
                lineRenderer.positionCount = i + 1;
                break;
            }
        }
        
        lineRenderer.SetPositions(points);
    }
}
