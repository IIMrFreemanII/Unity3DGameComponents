using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GameComponents.OdinLearning
{
    public class Player : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private PlayerState state = new PlayerState();

        [ShowInInspector]
        public float Health
        {
            get => state.health;
            set => state.health = value;
        }

        [ShowInInspector]
        public List<Item> Inventory
        {
            get => state.inventory;
            set => state.inventory = value;
        }

        private string storagePath = "Assets/GameComponents/OdinLearning/PlayerData.gameData";

        private void Start()
        {
            LoadState(storagePath);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveState(storagePath);
            }
        }

        public void SaveState(string filePath)
        {
            state.position = transform.position;
            byte[] bytes = SerializationUtility.SerializeValue(state, DataFormat.Binary);
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("Saved");
        }
        
        public void LoadState(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.Log($"Can't load file: {filePath}");
                return;
            } // No state to load
	
            byte[] bytes = File.ReadAllBytes(filePath);
            state = SerializationUtility.DeserializeValue<PlayerState>(bytes, DataFormat.Binary);
            transform.position = state.position;
        }

        // Serializable is unnecessary for Odin, but it allows for Unity to serialize the
        // PlayerState class in the editor.
        [Serializable]
        public class PlayerState
        {
            public Vector3 position;
            public float health;
            public List<Item> inventory;
        }
    }
    
    [Serializable]
    public class Item
    {
        public string itemId;
        public int count;
    }
}
