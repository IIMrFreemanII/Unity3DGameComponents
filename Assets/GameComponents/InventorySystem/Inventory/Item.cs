using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData;
using UnityEngine;

namespace GameComponents.InventorySystem.Inventory
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemDataSO itemDataSo = null;
        public ItemDataSO ItemDataSo => itemDataSo;
    }
}
