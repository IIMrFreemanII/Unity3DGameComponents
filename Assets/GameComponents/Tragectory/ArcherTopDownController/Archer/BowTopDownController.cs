using System;
using System.Collections;
using Extensions;
using GameComponents.Utils.PhysicsCalculations;
using UnityEngine;

namespace TopDownArcher
{
    [RequireComponent(typeof(ArcherTopDownController))]
    public class BowTopDownController : MonoBehaviour
    {
        private ArcherTopDownController archerController = null;
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private Arrow currentArrow = null;
        [SerializeField] private ArrowPosition arrowPosition = null;
        [SerializeField] private TrajectoryRenderer trajectoryRenderer = null;
        [SerializeField] private Bow bow = null;
        [SerializeField] private float reloadTime = 0.5f;
        [SerializeField] private Arrow arrowPrefab = null;
        [SerializeField] private float hDisplacement = 25f;
        [SerializeField] private bool debugPath = true;

        [SerializeField] private float gravity = -30f;

        private void Start()
        {
            archerController = GetComponent<ArcherTopDownController>();
            currentArrow = arrowPosition.GetComponentInChildren<Arrow>();
            gravity = Physics.gravity.y;
            trajectoryRenderer = GetComponent<TrajectoryRenderer>();
        }

        private void Update()
        {
            targetPosition = archerController.targetPosition;

            HandleHDisplacement();
            
            if (Input.GetMouseButtonDown(0))
            {
                Launch();
            }
            
            HandleBowRotation();

            if (debugPath)
            {
                DrawTrajectory();
            }
        }
        
        private void DrawTrajectory()
        {
            if (currentArrow)
            {
                Vector3 initialVelocity = PhysicsCalcUtil.CalcInitVelocity(currentArrow.transform.position,
                    targetPosition, hDisplacement, Physics.gravity.y).initialVelocity;
                
                Vector3 arrowRotationCompensator = currentArrow.transform.forward.With(0, z: 0);
                
                trajectoryRenderer.ShowTrajectory(currentArrow.transform.position, initialVelocity - arrowRotationCompensator);
            }
        }

        private void HandleHDisplacement()
        {
            if (!currentArrow) return;
            
            float maxHeight = (targetPosition - currentArrow.transform.position).y;
            hDisplacement = maxHeight < 0f ? 0f : maxHeight + 0.05f;
        }

        private void Launch()
        {
            currentArrow.transform.SetParent(null);
            
            currentArrow.rb.isKinematic = false;
            currentArrow.rb.velocity = PhysicsCalcUtil.CalcInitVelocity(currentArrow.transform.position,
                targetPosition, hDisplacement, Physics.gravity.y).initialVelocity;

            currentArrow = null;
            
            StartCoroutine(SetTimeout(reloadTime, () =>
            {
                currentArrow = Instantiate(arrowPrefab, arrowPosition.transform.position, arrowPosition.transform.rotation, arrowPosition.transform);
            }));
        }

        private void HandleBowRotation()
        {
            if (!currentArrow) return;
            
            Vector3 initialVelocity = PhysicsCalcUtil.CalcInitVelocity(currentArrow.transform.position,
                targetPosition, hDisplacement, Physics.gravity.y).initialVelocity;

            // simulate first step of arrow movement
            Vector3 startOfParabola = currentArrow.transform.position + initialVelocity * Time.fixedDeltaTime +
                                      Physics.gravity * (Time.fixedDeltaTime * Time.fixedDeltaTime) / 2f;
            // find direction 
            Vector3 lookDirection = startOfParabola - currentArrow.transform.position;
            
            // calc rotation
            bow.transform.forward = lookDirection;
            bow.transform.localEulerAngles = bow.transform.localEulerAngles.With(y: 0, z: 0);
        }
        
        IEnumerator SetTimeout(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
    }
}