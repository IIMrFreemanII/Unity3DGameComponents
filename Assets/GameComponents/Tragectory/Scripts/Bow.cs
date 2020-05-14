using System;
using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireForce = 30f;
    
    [SerializeField] private Arrow arrowPrefab = null;
    [SerializeField] private Transform arrowSpawnPosition = null;
    [SerializeField] private float reloadTime = 0.5f;

    [SerializeField] private Arrow currentArrow = null;

    [SerializeField] private Vector3 maxStretch = new Vector3(0, 0, -0.35f);

    [SerializeField] private bool isCancelStretching;
    
    [SerializeField] private float stretchStrength = 10f;
    [SerializeField] private float timeToResetCanceling = 1f;

    [SerializeField] private float currentStretchStrength;

    [SerializeField] private TrajectoryRenderer trajectoryRenderer;
    
    private bool IsInitialArrowPos => currentStretchStrength <= 0.05f;

    private void Start()
    {
        trajectoryRenderer = GetComponent<TrajectoryRenderer>();
        
        if (currentArrow == null)
        {
            currentArrow = Instantiate(arrowPrefab, arrowSpawnPosition.position, arrowSpawnPosition.rotation, arrowSpawnPosition);
        }
        
        Physics.gravity = new Vector3(0, -40f, 0);
    }

    private void Update()
    {
        CalculateStretchStrength();
        HandleTrajectory();
        
        HandleStretch();
    }

    private void HandleStretch()
    {
        if (currentArrow == null) return;
        
        Transform currentArrowTrans = currentArrow.transform;
        Vector3 currentArrowLocPos = currentArrowTrans.localPosition;

        if (!isCancelStretching)
        {
            if (Input.GetMouseButton(0))
            {
                currentArrowTrans.localPosition =
                    Vector3.Slerp(currentArrowLocPos, maxStretch, Time.deltaTime * stretchStrength);
                
                if (Input.GetMouseButtonDown(1))
                {
                    isCancelStretching = true;

                    StartCoroutine(ResetCancelStretchingWithDelay(timeToResetCanceling));
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Fire();
                }
            }
        }
        else
        {
            Vector3 startArrowPos = Vector3.zero;
            
            currentArrowTrans.localPosition =
                Vector3.Slerp(currentArrowLocPos, startArrowPos, Time.deltaTime * stretchStrength);
        }
    }

    IEnumerator ResetCancelStretchingWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isCancelStretching = false;
    }

    private void Fire()
    {
        if (currentArrow != null)
        {
            currentArrow.damage = damage * currentStretchStrength;
            currentArrow.transform.SetParent(null);

            currentArrow.Init(currentArrow.transform.forward * fireForce * currentStretchStrength);
            
            currentArrow = null;
            
            StartCoroutine(SetTimeout(reloadTime, () =>
            {
                currentArrow = Instantiate(arrowPrefab, arrowSpawnPosition.position, arrowSpawnPosition.rotation, arrowSpawnPosition);
            }));
        }
    }

    private void CalculateStretchStrength()
    {
        if (currentArrow == null) return;
        
        Vector3 currentArrowLocPos = currentArrow.transform.localPosition;

        float stretchStrengthPercent = (currentArrowLocPos.z / maxStretch.z) * 100f;
        currentStretchStrength = stretchStrengthPercent / 100f;
    }

    IEnumerator SetTimeout(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    private void HandleTrajectory()
    {
        if (currentArrow == null || IsInitialArrowPos)
        {
            trajectoryRenderer.Enabled = false;
            return;
        }

        if (!trajectoryRenderer.Enabled && !IsInitialArrowPos) trajectoryRenderer.Enabled = true;
        
        Transform currentArrowTransform = currentArrow.transform;
        Vector3 currentArrowPosition = currentArrowTransform.position;
        
        Vector3 arrowRotationCompensator = new Vector3(0, currentArrow.transform.forward.y, 0);
        
        Vector3 arrowVelocity = (currentArrowTransform.forward * fireForce * currentStretchStrength) - arrowRotationCompensator;
        trajectoryRenderer.ShowTrajectory(currentArrowPosition, arrowVelocity);
    }
}
