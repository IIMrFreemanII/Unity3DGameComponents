using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameComponents.OdinLearning.Training
{
    public class ExampleScript : SerializedMonoBehaviour
    {
        public ItemDataSO itemData;

        [ContextMenu("Show item path")]
        private void ShotAssetPath()
        {
            print(itemData.GetInstanceID());
            string path = AssetDatabase.GetAssetPath(itemData.GetInstanceID());
            print(path);
            print(AssetDatabase.LoadAssetAtPath<ItemDataSO>(path));
        }
    }
}