using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemDataSO;
using UnityEngine;

namespace GameComponents.InventorySystem.Inventory
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemDataSO itemDataSo = null;
        [SerializeField] private int amount = 1;
        
        public ItemDataSO ItemDataSo => itemDataSo;

        public int Amount
        {
            get => amount;
            set
            {
                if (value < 0)
                {
                    Debug.LogError("Item amount can't be below zero");
                    return;
                }

                if (!itemDataSo.IsStackable && value > 1)
                {
                    MoreThenOneWarning();
                    return;
                }

                amount = value;
            }
        }

        private void OnValidate()
        {
            if (!itemDataSo.IsStackable && amount > 1)
            {
                MoreThenOneWarning();
                amount = 1;
            }

            if (amount <= 0)
            {
                amount = 1;
            }
        }

        private void MoreThenOneWarning()
        {
            Debug.LogWarning($"{gameObject.name} can't have more then 1 because it isn't stackable!");
        }
    }
}
