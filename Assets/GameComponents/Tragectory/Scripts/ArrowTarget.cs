using GameComponents.Tragectory.Scripts;
using UnityEngine;

public class ArrowTarget : MonoBehaviour, IArrowTarget
{
    [SerializeField] private float impulseToHit = 2f;
    public float ImpulseToHit => impulseToHit;
}