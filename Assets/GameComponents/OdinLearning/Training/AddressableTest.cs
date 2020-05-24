using System;
using GameComponents.InventorySystem.Inventory.ScriptableObjects.ItemData;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableTest : SerializedMonoBehaviour
{
    [SerializeField]
    private AssetReference assetReference = null;

    public ItemDataSO itemDataSo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadAsset();
        }
    }

    // [ContextMenu("SerializeAssetRef")]
    private void LoadAsset()
    {
        // string json = JsonConvert.SerializeObject(assetReference, Formatting.Indented);
        string guid = assetReference.RuntimeKey.ToString();
        
        new AssetReference(guid).LoadAssetAsync<ItemDataSO>().Completed += operationHandle =>
        {
            itemDataSo = operationHandle.Result;
        };
    }
}
