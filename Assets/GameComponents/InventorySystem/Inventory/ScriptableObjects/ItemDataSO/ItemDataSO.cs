using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData
{
    [CreateAssetMenu(menuName = "Inventory/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        [SerializeField, AssetsOnly, Required]
        private Sprite icon = null;
        
        [SerializeField, Required]
        private string itemName = null;
        
        [SerializeField, AssetsOnly, Required]
        private Item playItemPrefab = null;

        [SerializeField, Required] private AssetReference assetReference = null;

        public string ItemName => itemName;
        public Item PlayItemPrefab => playItemPrefab;
        public Sprite Icon => icon;
        public AssetReference AssetReference => assetReference;
    }
}
