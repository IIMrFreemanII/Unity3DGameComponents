using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class HandleCutOffShader : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private readonly int _radius = Shader.PropertyToID("_Radius");
    private static readonly int Positions = Shader.PropertyToID("_Positions");
    
    public Color[] colors;
    public List<Color> points = null;

    [SerializeField] private float radius = 0.05f;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleMousePosition();
    }

    private int prevAmount;

    private void HandleMousePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();

                if (meshRenderer)
                {
                    print(meshRenderer.material.name);
                    
                    Color hitPosition = new Color(hit.point.x, hit.point.y, hit.point.z, 1f);
                    points.Add(hitPosition);
                    
                    int textureSize = 4;

                    int totalTexutreSize = textureSize * textureSize;
                    TextureFormat format = TextureFormat.RGBAFloat;
                    TextureWrapMode wrapMode = TextureWrapMode.Repeat;

                    Texture2D texture2D = new Texture2D(textureSize, textureSize, format, false);
                    texture2D.wrapMode = wrapMode;
                    
                    Color[] hitPositions = new Color[totalTexutreSize];

                    for (int i = 0; i < points.Count; i++)
                    {
                        hitPositions[i] = points[i];
                    }

                    // for (int i = 0; i < totalTexutreSize; i++)
                    // {
                    //     hitPositions[i] = points.First();
                    // }

                    // for (int y = 0; y < textureSize; y++)
                    // {
                    //     int yOffset = y * textureSize;
                    //     for (int x = 0; x < textureSize; x++)
                    //     {
                    //         hitPositions[x + yOffset] = points[y];
                    //     }
                    // }

                    // for (int i = 0; i < totalTexutreSize; i++)
                    // {
                    //     if (i < points.Count)
                    //     {
                    //         hitPositions[i] = points[i];
                    //     }
                    //     else
                    //     {
                    //         hitPositions[i] = new Color(0, 0, 0, 1);
                    //     }
                    // }

                    colors = hitPositions;
                    
                    texture2D.SetPixels(hitPositions);
                    texture2D.Apply();
                    
                    meshRenderer.material.SetTexture(Positions, texture2D);
                    meshRenderer.material.SetFloat(_radius, radius);
                }
            }
        }
    }
}
