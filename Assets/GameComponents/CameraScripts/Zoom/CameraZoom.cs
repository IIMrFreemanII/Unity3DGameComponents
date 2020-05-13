using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public bool zooming;
    public float zoomSpeed = 1f;
    public new Camera camera;

    void Update() {
        if (zooming) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            float zoomDistance = zoomSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            camera.transform.Translate(ray.direction * zoomDistance, Space.World);
        }
    }
}
