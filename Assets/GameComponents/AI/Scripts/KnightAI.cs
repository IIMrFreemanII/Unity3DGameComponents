using System;
using System.Linq;
using GameComponents.AI.AIStateMachine;
using GameComponents.AI.Scripts.KnightStates;
using GameComponents.Utils;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameComponents.AI.Scripts
{
    [RequireComponent(typeof(FieldOfView))]
    public class KnightAI : SerializedMonoBehaviour
    {
        public StateMachine StateMachine { get; private set; }
        
        [HideInInspector]
        public readonly int vertical = Animator.StringToHash("Vertical");
        
        [OdinSerialize] public Animator Animator { get; private set; }
        
        [OdinSerialize] public float LerpSpeed { get; private set; } = 5f;
        
        [OdinSerialize] public float MaxFindDist { get; private set; } = 5f;
        [OdinSerialize] public float MinFindDist { get; private set; } = -5f;
        
        public float CurrentVerticalMove { get; set; } = 0f;
        
        [OdinSerialize] public float MaxAxisMove { get; private set; } = 1f;
        [OdinSerialize] public float MinAxisMove { get; private set; } = 0f;
        [OdinSerialize] public float WaitTime { get; private set; } = 2f;
        
        public float ValueToMove { get; private set; } = 0.5f;
        public float ValueToRun { get; private set; } = 1f;

        public Vector3? PointToMove { get; set; } = null;
        
        private Timer timer = new Timer();

        private FieldOfView fieldOfView;
        public Transform currentTarget;

        [HideInInspector]
        public bool canAttack = true;

        private static readonly int Attack1 = Animator.StringToHash("Attack1");
        private static readonly int Attack2 = Animator.StringToHash("Attack2");
        private static readonly int Attack3 = Animator.StringToHash("Attack3");
        private static readonly int Attack4 = Animator.StringToHash("Attack4");
        private static readonly int Attack5 = Animator.StringToHash("Attack5");

        private int[] attackIds = {Attack1, Attack2, Attack3, Attack4, Attack5};

        private void Awake()
        {
            canAttack = true;
            
            fieldOfView = GetComponent<FieldOfView>();
            Animator = Animator ? Animator : GetComponent<Animator>();
            
            InitStateMachine();
        }

        private void InitStateMachine()
        {
            StateMachine = new StateMachine();

            void AddTransition(IState from, IState to, Func<bool> condition) =>
                StateMachine.AddTransition(from, to, condition);

            void AddAnyTransition(IState state, Func<bool> condition) =>
                StateMachine.AddAnyTransition(state, condition);
            
            Func<bool> ReachedMovePoint() => () => PointToMove == null;
            Func<bool> HasPointToMove() => () => PointToMove != null;
            Func<bool> IsTimeOutFinished() => () => timer.finished;
            Func<bool> IsStopped() => () => CurrentVerticalMove == MinAxisMove;
            Func<bool> HasTarget() => () => currentTarget != null;
            Func<bool> HasNoTarget() => () => currentTarget == null;

            Idle idle = new Idle(this, Animator);
            SearchPointToMove searchPointToMove = new SearchPointToMove(this);
            MoveToPoint moveToPoint = new MoveToPoint(this, Animator);
            TimeOut timeOut = new TimeOut(this, timer);
            Attack attack = new Attack(this);

            AddTransition(idle, timeOut, IsStopped());
            AddTransition(timeOut, searchPointToMove, IsTimeOutFinished());
            AddTransition(searchPointToMove, moveToPoint, HasPointToMove());
            AddTransition(moveToPoint, idle, ReachedMovePoint());
            AddTransition(attack, idle, HasNoTarget());
            
            AddAnyTransition(attack, HasTarget());
            
            StateMachine.SetEntryPoint(idle);
        }

        private void Update()
        {
           StateMachine.Tick(); 
           HandleCurrentTarget();
        }

        private void HandleCurrentTarget()
        {
            if (currentTarget == null)
            {
                if (fieldOfView.visibleTargets.Count > 0)
                {
                    currentTarget = fieldOfView.visibleTargets.First();
                }
            }
        }
        
        public void HandleAttack()
        {
            if (canAttack)
            {
                int attackId = Random.Range(0, attackIds.Length);
                Animator.SetTrigger(attackIds[attackId]);
                canAttack = false;
            }
        }

        private void AttackFinished()
        {
            canAttack = true;
        }
    }
}