using System;
using System.Collections.Generic;

namespace GameComponents.InventorySystem.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public Dictionary<SlotType, List<SlotData>> inventorySlotsData = new Dictionary<SlotType, List<SlotData>>();
    }
}