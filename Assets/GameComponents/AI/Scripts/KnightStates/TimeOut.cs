using GameComponents.AI.AIStateMachine;
using GameComponents.Utils;

namespace GameComponents.AI.Scripts.KnightStates
{
    public class TimeOut : IState
    {
        private KnightAI knightAi;
        private Timer timer;
        
        public TimeOut(KnightAI knightAi, Timer timer)
        {
            this.knightAi = knightAi;
            this.timer = timer;
        }
        
        public void Tick()
        {
            HandleTimeOut();
        }

        private void HandleTimeOut()
        {
            if (!timer.finished)
            {
                timer.Tick();
            }
        }

        public void OnEnter()
        {
            timer.SetTime(knightAi.WaitTime);
        }

        public void OnExit()
        {
        }
    }
}