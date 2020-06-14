using UnityEngine;

namespace GameComponents.AI.Scripts
{
    public class RootMotionController : MonoBehaviour
    {
        private Animator animator;
        private static readonly int MoveSpeedZ = Animator.StringToHash("MoveSpeedZ");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            float moveSpeedZ = animator.GetFloat(MoveSpeedZ);
            transform.Translate(Vector3.forward * (moveSpeedZ * Time.deltaTime));
        }
    }
}