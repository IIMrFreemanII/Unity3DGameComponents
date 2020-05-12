using System.Collections.Generic;
using UnityEngine;

public class HandleCutOffShader : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private readonly int _hitPosition = Shader.PropertyToID("_HitPosition");
    private readonly int _radius = Shader.PropertyToID("_Radius");

    [SerializeField] private List<Vector4> points;

    [SerializeField] private float radius = 0.8f;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleMousePosition();
    }

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
                    
                    Vector4 hitPosition = new Vector4(hit.point.x, hit.point.y, hit.point.z, 0f);
                    
                    meshRenderer.material.SetVector(_hitPosition, hitPosition);
                    meshRenderer.material.SetFloat(_radius, radius);
                }
            }
        }
    }
}
