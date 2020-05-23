using System;
using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData;
using Sirenix.OdinInspector;

namespace GameComponents.InventorySystem.Inventory
{
    public enum SlotType
    {
        Common,
        QuickPanel,
    }

    [Serializable]
    public class SlotData
    {
        [ShowInInspector, NonSerialized]
        public ItemDataSO itemDataSo;
        // itemId represents ItemDataSO id which allows to find this asset using AssetDatabase.GetAssetPath(assetId).
        public int itemDataSoId;
        public int itemsAmount;
    }
}
