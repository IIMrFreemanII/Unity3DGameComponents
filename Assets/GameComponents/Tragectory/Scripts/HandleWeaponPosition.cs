using GameComponents.FPSController;
using UnityEngine;

public class HandleWeaponPosition : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    
    void Start()
    {
        cam = Player.Instance.GetComponent<FPSController>().camera;
        
        transform.SetParent(cam.transform);
    }
}
