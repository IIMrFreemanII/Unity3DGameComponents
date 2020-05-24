using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace GameComponents.InventorySystem.Inventory
{
    public class InventoryUiSlot : MonoBehaviour
    {
        [SerializeField] private SlotType type = SlotType.Common;
        [SerializeField] private Image image = null;
        [SerializeField] private GameObject uiItemGO = null;
        [SerializeField] private SlotData slotData = null;

        public SlotType Type
        {
            get => type;
            set => type = value;
        }

        public Image Image => image;
        public GameObject UiItemGo => uiItemGO;
        public SlotData SlotData
        {
            get => slotData;
            set => slotData = value;
        }

        private void ActivateSlot(SlotData slotData)
        {
            Sprite sprite = slotData.itemDataSo.Icon;
            
            if (image.sprite != sprite)
            {
                image.sprite = sprite;
            }
            if (!uiItemGO.activeInHierarchy)
            {
                uiItemGO.SetActive(true);
            }
        }

        private void DeactivateSlot()
        {
            if (image.sprite)
            {
                image.sprite = null;
            }
            
            if (uiItemGO.activeInHierarchy)
            {
                uiItemGO.SetActive(false);
            }
        }

        public void UpdateUiSlot()
        {
            if (slotData.itemDataSoRef != null)
            {
                if (slotData.itemDataSo == null)
                {
                    AssetReference assetReference = slotData.itemDataSoRef;
                    
                    Addressables.LoadAssetAsync<ItemDataSO>(assetReference).Completed += handle =>
                    {
                        slotData.itemDataSo = handle.Result;

                        ActivateSlot(slotData);
                    };
                }
                else
                {
                    ActivateSlot(slotData);
                }
            }
            else
            {
                DeactivateSlot();
            }
        }
    }
}