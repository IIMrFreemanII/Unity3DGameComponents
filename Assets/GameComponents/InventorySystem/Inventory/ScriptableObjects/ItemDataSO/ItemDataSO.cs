using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemDataSO
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

        [SerializeField] private bool isStackable = default;
        [SerializeField, Required] private int maxAmount = default;
        

        public string ItemName => itemName;
        public Item PlayItemPrefab => playItemPrefab;
        public Sprite Icon => icon;
        public AssetReference AssetReference => assetReference;
        public bool IsStackable => isStackable;
        public int MaxAmount => maxAmount;

        private void OnValidate()
        {
            if (maxAmount <= 0 || !isStackable)
            {
                maxAmount = 1;
            }
        }
    }
}
