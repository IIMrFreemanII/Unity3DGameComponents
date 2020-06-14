using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameComponents.AI.Scripts
{
    public class FieldOfView : SerializedMonoBehaviour
    {
        public float viewRadius = 3f;
        [Range(0, 360)]
        public float viewAngle = 60f;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public float callPerSecond = 5f;
        
        public List<Transform> visibleTargets = new List<Transform>();
        private Teammate teammate;

        private void Awake()
        {
            teammate = GetComponent<Teammate>();
        }

        private void Start()
        {
            StartCoroutine(FindTargetsWithDelay());
        }

        private IEnumerator FindTargetsWithDelay()
        {
            while (true)
            {
                yield return new WaitForSeconds(1 / callPerSecond);
                FindVisibleTargets();
            }
        }

        private void FindVisibleTargets()
        {
            visibleTargets.Clear();
            
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;

                if (transform == target) continue;
                
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;
            
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}