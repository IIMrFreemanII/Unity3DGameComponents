using System;
using System.Collections.Generic;

namespace GameComponents.AI.AIStateMachine
{
    public class StateMachine
    {
        private IState currentState;

        private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> currentTransitions = new List<Transition>();
        private List<Transition> anyTransitions = new List<Transition>();

        private static List<Transition> EmptyTransitions = new List<Transition>(0);

        public void Tick()
        {
            Transition transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            currentState?.Tick();
        }

        private void SetState(IState state)
        {
            if (state == currentState) return;
            
            currentState?.OnExit();
            currentState = state;

            transitions.TryGetValue(currentState.GetType(), out currentTransitions);
            if (currentTransitions == null)
                currentTransitions = EmptyTransitions;
            
            currentState.OnEnter();
        }

        public void SetEntryPoint(IState state)
        {
            SetState(state);
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (transitions.TryGetValue(from.GetType(), out List<Transition> transitionsByType) == false)
            {
                transitionsByType = new List<Transition>();
                transitions[from.GetType()] = transitionsByType;
            }
            
            transitionsByType.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            anyTransitions.Add(new Transition(state, predicate));
        }
        
        private Transition GetTransition()
        {
            foreach (Transition anyTransition in anyTransitions)
                if (anyTransition.Condition())
                    return anyTransition;

            foreach (Transition currentTransition in currentTransitions)
                if (currentTransition.Condition())
                    return currentTransition;

            return null;
        }
    }
}