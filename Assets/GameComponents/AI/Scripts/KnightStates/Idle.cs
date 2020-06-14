using GameComponents.AI.AIStateMachine;
using UnityEngine;

namespace GameComponents.AI.Scripts.KnightStates
{
    public class Idle : IState
    {
        private KnightAI knightAi;
        private Animator animator;
        
        public Idle(KnightAI knightAi, Animator animator)
        {
            this.knightAi = knightAi;
            this.animator = animator;
        }
        
        public void Tick()
        {
            HandleIdle();
        }

        private void HandleIdle()
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
        { }

        public void OnExit()
        { }
    }
}