using GameComponents.Tragectory.Scripts;
using UnityEngine;

public class GroundLayer : MonoBehaviour, IArrowTarget
{
    [SerializeField] private float impulseToHit = 5f;
    public float ImpulseToHit => impulseToHit;
}
