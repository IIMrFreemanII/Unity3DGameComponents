using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemsData;
using UnityEngine;

namespace GameComponents.InventorySystem.Inventory
{
    public class GamePlayItem : MonoBehaviour
    {
        [SerializeField] private ItemDataSO itemDataSo = null;
        public ItemDataSO ItemDataSo => itemDataSo;
    }
}
