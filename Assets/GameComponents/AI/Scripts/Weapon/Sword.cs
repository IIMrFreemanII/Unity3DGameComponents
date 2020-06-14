using System;
using Extensions;
using UnityEngine;

namespace GameComponents.AI.Scripts.Weapon
{
    public class Sword : MonoBehaviour
    {
        private BoxCollider boxCollider;
        
        public float damage = 5f;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.HandleComponent<Teammate>(teammate =>
            {
                other.gameObject.HandleComponent<HealthController>(healthController =>
                {
                    healthController.Damage(damage);
                });
            });
        }

        public void DeactivateCollider()
        {
            boxCollider.enabled = false;
        }
        
        public void ActivateCollider()
        {
            boxCollider.enabled = true;
        }
    }
}
