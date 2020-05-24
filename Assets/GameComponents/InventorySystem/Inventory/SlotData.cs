using System;
using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

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
        [ShowInInspector, NonSerialized]
        public AssetReference itemDataSoRef;
        // Addressable asset GUID to find asset after loading json
        public string itemDataSoAssetGUID;
        public int itemsAmount;
    }
}
