using System.Collections;
using Extensions;
using GameComponents.Tragectory.Scripts;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody rb = null;
    public BoxCollider boxCollider = null;
    public float damage = 0;

    [SerializeField] private float rotationSpeed = 12f;

    [SerializeField] private float startMultiplier = -1f;
    [SerializeField] private float endMultiplier = 1.45f;

    private bool _isHitSomething;

    [SerializeField] private Vector3 drawOffset = Vector3.zero;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void Init(Vector3 startForce)
    {
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.AddForce(startForce, ForceMode.Impulse);
        boxCollider.isTrigger = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 drawStart = transform.position + (transform.forward * startMultiplier);
        Vector3 drawEnd = drawStart + (transform.forward * endMultiplier);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(drawStart + drawOffset, drawEnd + drawOffset);
    }
    
    private void RotateArrowInMovementDirection()
    {
        transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime * rotationSpeed);
    }

    private void FixedUpdate()
    {
        if (!_isHitSomething)
        {
            RotateArrowInMovementDirection();
        }
    }

    private IEnumerator DisableArrowWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableArrow();
    }

    private void DisableArrow()
    {
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rb.isKinematic = true;
        boxCollider.enabled = false;
        _isHitSomething = true;
    }

    private void HitTarget(IArrowTarget arrowTarget)
    {
        Component arrowTargetComponent = (Component) arrowTarget;
        print(arrowTargetComponent.name);
        
        DisableArrow();
        transform.SetParent(arrowTargetComponent.transform);
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject collidedGO = other.gameObject;
        float impulse = other.impulse.magnitude;
        
        print($"{collidedGO.name}: {impulse} impulse");

        collidedGO.HandleComponent<IArrowTarget>(arrowTarget =>
        {
            if (impulse > arrowTarget.ImpulseToHit)
            {
                HitTarget(arrowTarget);
            }
            else
            {
                _isHitSomething = true;
                StartCoroutine(DisableArrowWithDelay(2f));
            }
        });
    }
}
