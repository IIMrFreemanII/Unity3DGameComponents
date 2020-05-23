using Sirenix.OdinInspector;
using UnityEngine;

namespace GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData
{
    [CreateAssetMenu(menuName = "Inventory/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        [SerializeField][AssetsOnly][Required]
        private Sprite icon = null;
        
        [SerializeField][Required]
        private string itemName = null;
        
        [SerializeField][AssetsOnly][Required]
        private Item playItemPrefab = null;
    
        public string ItemName => itemName;
        public Item PlayItemPrefab => playItemPrefab;
        public Sprite Icon => icon;
    }
}
