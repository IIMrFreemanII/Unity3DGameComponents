using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameComponents.InventorySystem.Inventory.ScriptableObjects.InventoryDataSO
{
    [CreateAssetMenu(menuName = "Inventory/InventoryData")]
    public class InventorySO : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<SlotType, InventorySlotsData> slotsData = new Dictionary<SlotType, InventorySlotsData>();
        public Dictionary<SlotType, InventorySlotsData> SlotsData => slotsData;
    }

    [Serializable]
    public class InventorySlotsData
    {
        public int amount;
        public InventoryUiSlot slotPrefab;
    }
}