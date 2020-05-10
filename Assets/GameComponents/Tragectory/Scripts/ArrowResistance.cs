using UnityEngine;

namespace GameComponents.Tragectory.Scripts
{
    public class ArrowResistance : MonoBehaviour, IArrowTarget
    {
        [SerializeField] private float impulseToHit = 40f;
        public float ImpulseToHit => impulseToHit;
    }
}