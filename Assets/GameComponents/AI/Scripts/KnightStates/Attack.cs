using Extensions;
using GameComponents.AI.AIStateMachine;
using UnityEngine;

namespace GameComponents.AI.Scripts.KnightStates
{
    public class Attack : IState
    {
        private Animator animator;
        private KnightAI knightAi;
        private Transform transform;

        public Attack(KnightAI knightAi)
        {
            this.knightAi = knightAi;
            animator = knightAi.Animator;
            transform = knightAi.transform;
        }
        
        public void Tick()
        {
            HandleChaseTarget();
        }

        private void HandleChaseTarget()
        {
            if (knightAi.currentTarget)
            {
                if (!ReachedDestination(knightAi.currentTarget.position))
                {
                    HandleRun(knightAi.currentTarget.position);
                }
                else
                {
                    HandleRotation(knightAi.currentTarget.position);
                    HandleStop();
                    knightAi.HandleAttack();
                }
            }
        }

        private void HandleRun(Vector3 dest)
        {
            HandleRotation(dest);
            
            knightAi.CurrentVerticalMove = Mathf.Lerp(knightAi.CurrentVerticalMove, knightAi.ValueToRun,
                Time.deltaTime * knightAi.LerpSpeed);
            
            animator.SetFloat(knightAi.vertical, knightAi.CurrentVerticalMove);
        }

        private void HandleRotation(Vector3 dest)
        {
            Vector3 moveDir = transform.DirectionTo(dest);
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * knightAi.LerpSpeed).With(y: 0f);
        }
        
        private bool ReachedDestination(Vector3 dest)
        {
            float distToDest = Vector3.Distance(dest, transform.position);
                
            if (distToDest > 1.9f) return false;
                
            return true;
        }

        private void HandleStop()
        {
            if (knightAi.CurrentVerticalMove <= 0.05f)
            {
                knightAi.CurrentVerticalMove = knightAi.MinAxisMove;
            }
            else if (knightAi.CurrentVerticalMove > 0.05f)
            {
                knightAi.CurrentVerticalMove =
                    Mathf.Lerp(knightAi.CurrentVerticalMove, knightAi.MinAxisMove, Time.deltaTime * 8f);
            }
            
            animator.SetFloat(knightAi.vertical, knightAi.CurrentVerticalMove);
            
        }
        
        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}