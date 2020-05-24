using Extensions;
using GameComponents.InventorySystem.Inventory;
using UnityEngine;

public class GrabItems : MonoBehaviour
{
    [SerializeField] private Camera cam = null;
    private Transform camTransform;
    
    [SerializeField] private Rigidbody grabbedObjRb = null;
    
    [SerializeField] private GrabPosition grabPosition = null;
    private Transform grabTransformPosition;
    
    [SerializeField] private float maxRaycastDist = 5f;
    [SerializeField] private float grabSpeed = 10f;

    private void Start()
    {
        cam = Camera.main;
        camTransform = cam.transform;

        grabPosition = GetComponentInChildren<GrabPosition>();
        grabTransformPosition = grabPosition.transform;
        grabTransformPosition.SetParent(camTransform);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HandleExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void HandleCursor()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        HandleExit();
        HandleCursor();
        HandleGrab();   
    }

    private void HandleGrab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 camPos = camTransform.position;
            Vector3 camLookDir = camTransform.forward;

            if (Physics.Raycast(camPos, camLookDir, out RaycastHit hit, maxRaycastDist))
            {
                hit.transform.gameObject.HandleComponent<Rigidbody>(rb => grabbedObjRb = rb);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            grabbedObjRb = null;
        }

        if (grabbedObjRb)
        {
            grabbedObjRb.velocity = grabSpeed * (grabTransformPosition.position - grabbedObjRb.position);
        }
    }
}
