using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemDataSO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameComponents.OdinLearning.Training
{
    public class ExampleScript : SerializedMonoBehaviour
    {
        public ItemDataSO itemData;

#if UNITY_EDITOR
        [ContextMenu("Show item path")]
        private void ShotAssetPath()
        {
            print(itemData.GetInstanceID());
            string path = AssetDatabase.GetAssetPath(itemData.GetInstanceID());
            print(path);
            print(AssetDatabase.LoadAssetAtPath<ItemDataSO>(path));
        }
#endif
    }
}