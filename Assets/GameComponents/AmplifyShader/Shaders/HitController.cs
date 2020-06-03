using Extensions;
using UnityEngine;

namespace GameComponents.AmplifyShader.Shaders
{
    public class HitController : MonoBehaviour
    {
        private Camera cam;

        public float maxHitRadius = 0.1f;
        public float minHitRadius = 0.5f;

        private void Start()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            HandleHit();
        }

        private void HandleHit()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    hit.transform.HandleComponent<HandleHit>(component =>
                    {
                        Vector3 localPos = component.transform.InverseTransformPoint(hit.point);

                        Vector4 localHitPoint = new Vector4(localPos.x, localPos.y, localPos.z,
                            Random.Range(minHitRadius, maxHitRadius));
                        
                        component.AddHitPoint(localHitPoint);
                    });
                }
            }
        }
    }
}