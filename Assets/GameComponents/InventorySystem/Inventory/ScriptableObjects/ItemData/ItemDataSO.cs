using UnityEngine;

namespace GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemsData
{
    [CreateAssetMenu(menuName = "Inventory/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        [SerializeField] private Sprite icon = null;
        [SerializeField] private string itemName = null;
        [SerializeField] private GamePlayItem playItemPrefab = null;
    
        public string ItemName => itemName;
        public GamePlayItem PlayItemPrefab => playItemPrefab;
        public Sprite Icon => icon;
    }
}
