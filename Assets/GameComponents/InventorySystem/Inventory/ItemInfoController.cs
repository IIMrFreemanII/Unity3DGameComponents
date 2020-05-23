using Extensions;
using GameComponents.InventorySystem.Inventory;
using GameComponents.Utils;
using UnityEngine;

public class ItemInfoController : MonoBehaviour
{
    [SerializeField] private Camera cam = null;
    private Transform camTransform;
    
    [SerializeField] private ItemInfo itemInfo = null;
    private RectTransform itemInfoRectTrans;
    
    [SerializeField] private float maxRaycastDist = 5f;

    [SerializeField] private Vector3 itemInfoPosOffset = new Vector3(20f, 20f);

    private Item currentHoveredPlayItem;
    
    [SerializeField] private float rayCastsPerSecond = 1f;
    
    private void Start()
    {
        cam = Camera.main;
        camTransform = cam.transform;

        itemInfoRectTrans = itemInfo.GetComponent<RectTransform>();
    }

    private void Update()
    {
        TimeUtils.SetInterval(HandleHoverOverGameItem, rayCastsPerSecond);
    }

    private void ResetItemInfo()
    {
        currentHoveredPlayItem = null;
        itemInfo.gameObject.SetActive(false);
    }

    private void HandleHoverOverGameItem()
    {
        Vector3 camPos = camTransform.position;
        Vector3 camLookDir = camTransform.forward;
        
        if (Physics.Raycast(camPos, camLookDir, out RaycastHit hit, maxRaycastDist))
        {
            bool hasTargetComponent = hit.transform.HandleComponent<Item>(gameItem =>
            {
                if (currentHoveredPlayItem != gameItem)
                {
                    currentHoveredPlayItem = gameItem;
                    
                    Vector3 hitPos = hit.point;
                    Vector3 screenPos = cam.WorldToScreenPoint(hitPos);
                    
                    itemInfo.Text = gameItem.name;

                    itemInfo.gameObject.SetActive(true);
                    itemInfoRectTrans.position = screenPos.With(z: 0) + itemInfoPosOffset.With(z: 0);
                }
                
            });

            if (!hasTargetComponent)
            {
                ResetItemInfo();
            }
        }
        else
        {
            ResetItemInfo();
        }
    }
}
