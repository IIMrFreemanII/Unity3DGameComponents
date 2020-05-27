using System;
using System.Collections.Generic;
using Extensions;
using GameComponents.InventorySystem.Inventory;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using SlotData = GameComponents.InventorySystem.Inventory.SlotData;

public class InventoryController : SerializedMonoBehaviour
{
    [SerializeField] private Camera cam = null;
    [SerializeField] private float maxDistToPickUp = 5f;
    [SerializeField] private InventoryUiContainer inventoryUiContainer = null;

    [OdinSerialize]
    private InventoryUiSlot slotPrefab = null;
    [NonSerialized, OdinSerialize]
    public InventoryData inventoryData;
    [SerializeField]
    private Dictionary<SlotType, List<InventoryUiSlot>> uiSlots;

    private string fileName = "inventoryData.json";
    private string filePath;

    private Transform camTrans;
    
    private void Awake()
    {
        filePath = $"{Application.persistentDataPath}/{fileName}";
        cam = Camera.main;
        camTrans = cam.transform;
    }

    private void Start()
    {
        InitInvData();
    }

    private void Update()
    {
        HandlePickUpItem();
    }

    private void InitInvData()
    {
        InventoryData invData = inventoryData.LoadInventoryData(filePath);

        if (invData == null)
        {
            Debug.Log("No saved data, initializing default data");
            InitUiSlots(inventoryData);
        }
        else
        {
            inventoryData = invData;
            InitUiSlots(inventoryData);
        }
    }

    private void InitUiSlots(InventoryData invData)
    {
        uiSlots = new Dictionary<SlotType, List<InventoryUiSlot>>();
        
        foreach (KeyValuePair<SlotType,List<SlotData>> keyValuePair in invData.inventorySlotsData)
        {
            uiSlots.Add(keyValuePair.Key, new List<InventoryUiSlot>());
            
            foreach (SlotData slotData in keyValuePair.Value)
            {
                InventoryUiSlot uiSlot = Instantiate(slotPrefab, inventoryUiContainer.transform);
                uiSlot.SlotData = slotData;
                uiSlots[keyValuePair.Key].Add(uiSlot);
            }
        }
        
        UpdateUiSlots();
    }

    private void HandlePickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 camPos = camTrans.position;
            Vector3 camDir = camTrans.forward;
    
            if (Physics.Raycast(camPos, camDir, out RaycastHit hit, maxDistToPickUp))
            {
                hit.transform.HandleComponent<Item>(item =>
                {
                    AddItem(item, SlotType.Common);
                });
            }
        }
    }

    private void AddItem(Item item, SlotType slotType)
    {
        inventoryData.AddItem(item, slotType);

        UpdateUiSlots();
        inventoryData.SaveInventoryData(filePath);
        print($"add {item.ItemDataSo.ItemName}");
    }

    private void UpdateUiSlots()
    {
        foreach (KeyValuePair<SlotType,List<InventoryUiSlot>> keyValuePair in uiSlots)
        {
            foreach (InventoryUiSlot inventoryUiSlot in keyValuePair.Value)
            {
                inventoryUiSlot.UpdateUiSlot();
            }
        }
    }
}
