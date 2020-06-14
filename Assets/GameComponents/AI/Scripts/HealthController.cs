using UnityEngine;

namespace GameComponents.AI.Scripts
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private float health;

        public void Damage(float damage)
        {
            health -= damage;
            // Debug.Log($"{gameObject.name} health: {health}");
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}