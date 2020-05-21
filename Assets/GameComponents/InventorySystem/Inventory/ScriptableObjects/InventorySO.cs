using System;
using System.Collections.Generic;
using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemsData;
using UnityEngine;

namespace GameComponents.InventorySystem.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Inventory/InventoryData")]
    public class InventorySO : ScriptableObject
    {
        public List<SlotData> commonSlots = new List<SlotData>();

        // public void ClearData()
        // {
        //     // itemsData.Clear();
        //     commonSlots.Clear();
        // }

        private void OnValidate()
        {
            // string[] assets = AssetDatabase.FindAssets("t:UiItem UiItem");
            //
            // foreach (string assetGUID in assets)
            // {
            //     Debug.Log(AssetDatabase.GUIDToAssetPath(assetGUID));
            // }
        }

        public event Action<List<SlotData>> OnItemAdd;

        public void AddSlot(int orderNumber, SlotType slotType)
        {
            SlotData slotData = new SlotData(orderNumber, slotType);
            if (slotType == SlotType.Common)
            {
                if (orderNumber <= commonSlots.Count - 1) return;
                
                commonSlots.Add(slotData);
            }
        }
        
        public void AddItem(ItemDataSO itemDataSo, SlotType type)
        {
            SlotData slotData = FindEmptySlot(type);
            if (slotData != null)
            {
                slotData.itemDataSo = itemDataSo;
                slotData.itemsAmount = 1;
                OnItemAdd?.Invoke(commonSlots);
            }
            else
            {
                Debug.LogWarning("slotData is null");
            }
        }

        public void InitUi()
        {
            OnItemAdd?.Invoke(commonSlots);
        }

        // public void InitItemsData()
        // {
        //     if (slotsData.Count > 0 && itemsData.Count > 0)
        //     {
        //         for (int i = 0; i < itemsData.Count; i++)
        //         {
        //             ItemData itemData = itemsData[i];
        //             SlotData slotData = slotsData[i];
        //             slotData.itemData = itemData;
        //         }
        //         
        //         OnItemAdd?.Invoke();
        //     }
        //     else
        //     {
        //         Debug.LogWarning("slotsData or itemsData is empty");
        //     }
        //     
        // }

        private SlotData FindEmptySlot(SlotType type)
        {
            if (type == SlotType.Common)
            {
                foreach (SlotData commonSlot in commonSlots)
                {
                    Debug.Log(commonSlot.itemDataSo);
                    
                    if (commonSlot.itemDataSo == null)
                    {
                        return commonSlot;
                    }
                }
            }
            return null;
        }
    }

    public enum SlotType
    {
        Common,
        QuickPanel,
    }

    [Serializable]
    public class SlotData
    {
        public int orderNumber;
        public SlotType type;
        public ItemDataSO itemDataSo;
        public int itemsAmount;
        
        public SlotData(int orderNumber, SlotType type)
        {
            this.orderNumber = orderNumber;
            this.type = type;
        }
    }
}