using UnityEngine;
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

        public void UpdateUiSlot()
        {
            if (slotData.itemDataSo != null)
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
            else
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
        }
    }
}