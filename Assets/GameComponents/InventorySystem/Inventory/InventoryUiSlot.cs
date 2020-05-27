using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemDataSO;
using TMPro;
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
        [SerializeField] private TMP_Text tmpText = null;

        public SlotType Type
        {
            get => type;
            set => type = value;
        }

        private TMP_Text TmpText => tmpText;

        public Image Image => image;

        private Sprite Sprite
        {
            get => image.sprite;
            set
            {
                if (image.sprite != value)
                {
                    image.sprite = value;
                }
            }
        }
        public GameObject UiItemGo => uiItemGO;
        public SlotData SlotData
        {
            get => slotData;
            set => slotData = value;
        }

        private void ActivateSlot(SlotData slotData)
        {
            Sprite sprite = slotData.itemDataSo.Icon;
            
            // handle sprite
            if (image.sprite != sprite)
            {
                image.sprite = sprite;
            }
            
            // handle slot gameObject
            if (!uiItemGO.activeInHierarchy)
            {
                uiItemGO.SetActive(true);
            }
            
            HandleText(slotData);
        }

        private void HandleText(SlotData slotData)
        {
            if (slotData.itemsAmount > 1)
            {
                string newAmount = slotData.itemsAmount.ToString();

                if (TmpText.text != newAmount)
                {
                    TmpText.text = newAmount;
                }
                
                if (!TmpText.gameObject.activeInHierarchy)
                {
                    TmpText.gameObject.SetActive(true);
                }
            }
            else
            {
                if (TmpText.gameObject.activeInHierarchy)
                {
                    TmpText.gameObject.SetActive(false);
                }
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