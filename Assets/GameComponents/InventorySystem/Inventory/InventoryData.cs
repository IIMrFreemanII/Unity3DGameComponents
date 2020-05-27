using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace GameComponents.InventorySystem.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public Dictionary<SlotType, List<SlotData>> inventorySlotsData = new Dictionary<SlotType, List<SlotData>>();

        public SlotData GetEmptySlot(SlotType type)
        {
            List<SlotData> slotsByType = inventorySlotsData[type];

            return slotsByType.Find(slot => slot.itemDataSo == null);
        }

        public SlotData GetSlotWithItem(SlotType type, Item item)
        {
            List<SlotData> slotsByType = inventorySlotsData[type];

            SlotData result = slotsByType.Find(slot =>
                slot.itemDataSo &&
                slot.itemDataSo.IsStackable && 
                slot.itemDataSo == item.ItemDataSo &&
                slot.itemsAmount < slot.itemDataSo.MaxAmount
            ) ?? GetEmptySlot(type);

            return result;
        }

        public void AddItem(Item item, SlotType slotType)
        {
            if (item.ItemDataSo.IsStackable)
            {
                DistributeItemsAmount(item, slotType);
            }
            else
            {
                SlotData slotData = GetEmptySlot(slotType);
                
                if (slotData == null)
                {
                    Debug.LogWarning("No free slots");
                    return;
                }
                
                slotData.itemsAmount++;
                slotData.itemDataSo = item.ItemDataSo;
                AddRef(item, slotData);
                Object.Destroy(item.gameObject);
            }
        }
        
        private void AddRef(Item item, SlotData slotData)
        {
            if (item.ItemDataSo.AssetReference != null)
            {
                slotData.itemDataSoRef = item.ItemDataSo.AssetReference;
                slotData.itemDataSoAssetGUID = item.ItemDataSo.AssetReference.RuntimeKey.ToString();
            }
            else
            {
                Debug.LogError("No asset reference");
            }
        }

        private void DistributeItemsAmount(Item item, SlotType slotType)
        {
            SlotData slotData = GetSlotWithItem(slotType, item);

            if (slotData == null)
            {
                Debug.LogWarning("No free slots");
                return;
            }

            int freeSpace;

            if (slotData.itemDataSo == null)
            {
                freeSpace = item.ItemDataSo.MaxAmount - slotData.itemsAmount;
            }
            else
            {
                freeSpace = slotData.itemDataSo.MaxAmount - slotData.itemsAmount;
            }

            if (freeSpace >= item.Amount)
            {
                slotData.itemsAmount += item.Amount;
                AddRef(item, slotData);
                Object.Destroy(item.gameObject);
            }
            else
            {
                int maxAmountCanAdd = freeSpace;

                slotData.itemsAmount += maxAmountCanAdd;
                item.Amount -= maxAmountCanAdd;
                slotData.itemDataSo = item.ItemDataSo;
                
                AddRef(item, slotData);
                
                DistributeItemsAmount(item, slotType);
            }
        }

        public void SaveInventoryData(string filePath)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
            Debug.Log("Save success");
        }
        
        public InventoryData LoadInventoryData(string filePath)
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
                    if (slotData.itemDataSoAssetGUID != null)
                    {
                        slotData.itemDataSoRef = new AssetReference(slotData.itemDataSoAssetGUID);
                    }
                }
            }

            Debug.Log($"Load file {filePath} success");
        
            return invData;
        }
    }
}