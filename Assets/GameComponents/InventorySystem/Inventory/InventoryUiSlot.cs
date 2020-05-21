using GameComponents.InventorySystem.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace GameComponents.InventorySystem.Inventory
{
    public class InventoryUiSlot : MonoBehaviour
    {
        [SerializeField] private SlotType type = SlotType.Common;
        [SerializeField] private Image image = null;
        [SerializeField] private GameObject uiItemGO = null;
        public SlotType Type => type;
        public Image Image => image;
        public GameObject UiItemGo => uiItemGO;
    }
}
