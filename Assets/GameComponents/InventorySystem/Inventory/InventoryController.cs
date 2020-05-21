using Extensions;
using GameComponents.InventorySystem.Inventory;
using GameComponents.InventorySystem.Inventory.ScriptableObjects;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventorySO inventorySo = null;
    [SerializeField] private Transform inventoryContainerTrans = null;
    [SerializeField] private Camera cam = null;
    [SerializeField] private float maxDistToPickUp = 5f;
    
    private Transform camTrans;

    private void Awake()
    {
        cam = Camera.main;
        camTrans = cam.transform;
    }

    private void Start()
    {
        // InitSlots();
    }

    private void Update()
    {
        HandlePickUpItem();
    }

    private void InitSlots()
    {
        for (int i = 0; i < inventoryContainerTrans.childCount; i++)
        {
            InventoryUiSlot uiSlot = inventoryContainerTrans.GetChild(i).GetComponent<InventoryUiSlot>();
            inventorySo.AddSlot(i, uiSlot.Type);
        }
    }

    private void HandlePickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 camPos = camTrans.position;
            Vector3 camDir = camTrans.forward;

            if (Physics.Raycast(camPos, camDir, out RaycastHit hit, maxDistToPickUp))
            {
                hit.transform.HandleComponent<GamePlayItem>(item =>
                {
                    inventorySo.AddItem(item.ItemDataSo, SlotType.Common);
                    Destroy(item.gameObject);
                });
            }
        }
    }
    
}
