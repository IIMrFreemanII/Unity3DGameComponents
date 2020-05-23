using System;
using System.Collections.Generic;
using System.IO;
using Extensions;
using GameComponents.InventorySystem.Inventory;
using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using SlotData = GameComponents.InventorySystem.Inventory.SlotData;

public class InventoryController : SerializedMonoBehaviour
{
    [SerializeField] private Camera cam = null;
    [SerializeField] private float maxDistToPickUp = 5f;
    [SerializeField] private InventoryUiContainer inventoryUiContainer = null;

    [SerializeField] 
    private InventoryUiSlot slotPrefab = null;
    [NonSerialized, OdinSerialize]
    public InventoryData inventoryData;
    [SerializeField]
    private Dictionary<SlotType, List<InventoryUiSlot>> uiSlots;

    [SerializeField] private string filePath = null;

    private Transform camTrans;
    
    private void Awake()
    {
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
        InventoryData invData = LoadInventoryData(filePath);

        if (invData == null)
        {
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
                    Destroy(item.gameObject);
                });
            }
        }
    }

    private SlotData GetEmptySlot(SlotType slotType)
    {
        foreach (SlotData slotData in inventoryData.inventorySlotsData[slotType])
        {
            if (slotData.itemDataSoId == 0)
            {
                return slotData;
            }
        }

        Debug.Log("No free slots");
        return null;
    }

    private void AddItem(Item item, SlotType slotType)
    {
        SlotData slotData = GetEmptySlot(slotType);
        slotData.itemsAmount++;
        slotData.itemDataSo = item.ItemDataSo;
        slotData.itemDataSoId = item.ItemDataSo.GetInstanceID();
        
        SaveInventoryData(filePath);
        
        UpdateUiSlots();
        print($"add {item.ItemDataSo.ItemName}");
    }

    private void SaveInventoryData(string filePath)
    {
        string json = JsonConvert.SerializeObject(inventoryData, Formatting.Indented);
        File.WriteAllText(filePath, json);
        Debug.Log("Save success");
    }

    private InventoryData LoadInventoryData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.Log($"Can't load file: {filePath}");
            return null;
        }
        
        string json = File.ReadAllText(filePath);
        InventoryData invData = JsonConvert.DeserializeObject<InventoryData>(json);

        foreach (KeyValuePair<SlotType,List<SlotData>> keyValuePair in invData.inventorySlotsData)
        {
            foreach (SlotData slotData in keyValuePair.Value)
            {
                string assetPath = AssetDatabase.GetAssetPath(slotData.itemDataSoId);
                slotData.itemDataSo = AssetDatabase.LoadAssetAtPath<ItemDataSO>(assetPath);
            }
        }

        Debug.Log($"Load file {filePath} success");
        
        return invData;
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
