using System.Collections.Generic;
using GameComponents.InventorySystem.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace GameComponents.InventorySystem.Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        // [SerializeField] private Canvas inventoryCanvas = null;
        // [SerializeField] private RectTransform inventoryContainer = null;
        // [SerializeField] private List<InventoryUiSlot> inventoryUiSlots = new List<InventoryUiSlot>();
        //
        // [SerializeField] private InventorySO inventorySo = null;

        // private void Start()
        // {
        //     for (int i = 0; i < inventoryContainer.childCount; i++)
        //     {
        //         inventoryUiSlots.Add(inventoryContainer.GetChild(i).GetComponent<InventoryUiSlot>());
        //     }
        //     
        //     inventorySo.InitUi();
        // }
        //
        // private void OnEnable()
        // {
        //     inventorySo.OnItemAdd += DrawItems;
        // }
        //
        // private void OnDisable()
        // {
        //     inventorySo.OnItemAdd -= DrawItems;
        // }
        //
        // private void DrawItems(List<SlotData> slotsData)
        // {
        //     foreach (SlotData slotData in slotsData)
        //     {
        //         ItemDataSO itemDataSo = slotData.itemDataSo;
        //         if (itemDataSo != null)
        //         {
        //             InventoryUiSlot inventoryUiSlot = inventoryUiSlots[slotData.orderNumber];
        //             Image imageComponent = inventoryUiSlot.Image;
        //             
        //             imageComponent.sprite = itemDataSo.Icon;
        //             inventoryUiSlot.UiItemGo.SetActive(true);
        //         }
        //     }
        // }
    }
}
