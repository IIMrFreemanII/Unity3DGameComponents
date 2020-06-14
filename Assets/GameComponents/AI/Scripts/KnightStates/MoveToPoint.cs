using Extensions;
using GameComponents.AI.AIStateMachine;
using UnityEngine;

namespace GameComponents.AI.Scripts.KnightStates
{
    public class MoveToPoint : IState
    {
        private KnightAI knightAi;
        private Animator animator;
        private Transform transform;
        
        public MoveToPoint(KnightAI knightAi, Animator animator)
        {
            this.knightAi = knightAi;
            this.animator = animator;
            transform = knightAi.transform;
        }

        public void Tick()
        {
            HandleMoveToPoint();
        }

        private void HandleMoveToPoint()
        {
            if (knightAi.PointToMove.HasValue)
            {
                if (!ReachedDestination())
                {
                    HandleWalk(knightAi.PointToMove.Value);
                }
                else
                {
                    knightAi.PointToMove = null;
                }
            }
        }
        
        private void HandleWalk(Vector3 dest)
        {
            Vector3 moveDir = transform.DirectionTo(dest);
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * knightAi.LerpSpeed);

            knightAi.CurrentVerticalMove = Mathf.Lerp(knightAi.CurrentVerticalMove, knightAi.ValueToMove,
                Time.deltaTime * knightAi.LerpSpeed);
            
            animator.SetFloat(knightAi.vertical, knightAi.CurrentVerticalMove);
        }

        private bool ReachedDestination()
        {
            if (knightAi.PointToMove.HasValue)
            {
                float distToDest = Vector3.Distance(knightAi.PointToMove.Value, transform.position);
                
                if (distToDest > 1f) return false;
                
                return true;
            }

            return false;
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}