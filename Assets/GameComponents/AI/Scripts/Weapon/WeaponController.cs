using UnityEngine;

namespace GameComponents.AI.Scripts.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Sword sword;
        
        public void DeactivateCollider()
        {
            sword.DeactivateCollider();
        }
        
        public void ActivateCollider()
        {
            sword.ActivateCollider();
        }
    }
}
