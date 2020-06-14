using GameComponents.AI.AIStateMachine;
using UnityEngine;

namespace GameComponents.AI.Scripts.KnightStates
{
    public class SearchPointToMove : IState
    {
        private KnightAI knightAi;
        private Transform transform;
        
        public SearchPointToMove(KnightAI knightAi)
        {
            this.knightAi = knightAi;
            transform = knightAi.transform;
        }

        public void Tick()
        {
            if (knightAi.PointToMove == null)
            {
                SearchRandomPointToMove();
            }
            else
            {
                Debug.LogWarning("PointToMove isn't null.");
            }
        }

        private void SearchRandomPointToMove()
        {
            Vector3 currentPos = transform.position;
            float randomOffsetX = currentPos.x + Random.Range(knightAi.MinFindDist, knightAi.MaxFindDist);
            float randomOffsetZ = currentPos.z + Random.Range(knightAi.MinFindDist, knightAi.MaxFindDist);

            knightAi.PointToMove = new Vector3(randomOffsetX, currentPos.y, randomOffsetZ);
        }

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }
    }
}