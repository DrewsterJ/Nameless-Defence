using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Public fields
    public int fireRate; // in seconds
    public float fireRange; // range the turret can shoot
    public int damage; // per bullet
    public GameObject bulletPrefab; // projectile the turret shoots
    
    // Private fields
    private const float _aimAngleOffset = -90f; // offset for aiming at an enemy
    private GameObject _activeTarget; // what the turret is shooting at
    private Coroutine _firingCoroutine; // controls when this turret shoots
    
    void Start()
    {
        Debug.Assert(fireRate > 0);
        Debug.Assert(damage >= 0);
        Debug.Assert(_activeTarget == null);
        Debug.Assert(fireRange > 0);
    }
    
    void Update()
    {
        if (!GameManager.instance._gameActive)
            return;
        
        HandleActiveTargetEngagements();
    }
    
    // Method to try engaging a given target (called by the GameManager)
    public void TryEngageTarget(GameObject target)
    {
        if (IsEngagingTarget() || !IsValidTarget(target))
            return;

        if (IsWithinRange(target))
        {
            DisengageActiveTarget();
            EngageTarget(target);
        }
    }

    // Returns whether this turret is engaging a target
    public bool IsEngagingTarget()
    {
        return (_activeTarget != null && !_activeTarget.IsUnityNull() && !_activeTarget.IsDestroyed());
    }

    // Returns whether the given target can be engaged
    private bool IsValidTarget(GameObject target)
    {
        return (target != null && !target.IsUnityNull() && !target.IsDestroyed());
    }
    
    // Handles engagements with an active target
    private void HandleActiveTargetEngagements()
    {
        if (!IsEngagingTarget())
            return;

        if (IsWithinRange(_activeTarget))
            AimAtTarget(_activeTarget);
        else
            DisengageActiveTarget();
    }

    // Method to start firing at `fireRate`
    private void StartFiring()
    {
        _firingCoroutine = StartCoroutine(FireAtInterval(fireRate));
    }
    
    // Method to stop firing
    private void StopFiring()
    {
        if (_firingCoroutine == null)
            return;
        
        StopCoroutine(_firingCoroutine);
        _firingCoroutine = null;
    }
    
    // Method to aim at a given target
    private void AimAtTarget(GameObject target)
    {
        if (!IsValidTarget(target))
            return;
        
        // Source (from RDSquare): https://discussions.unity.com/t/how-do-i-rotate-a-2d-object-to-face-another-object/187072/2
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x -transform.position.x ) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimAngleOffset));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360);
    }

    // Method to actively follow and shoot at a given target
    private void EngageTarget(GameObject target)
    {
        Debug.Assert(IsValidTarget(target));
        
        _activeTarget = target;
        StartFiring();
    }
    
    // Method to stop following and shooting at a given target
    public void DisengageActiveTarget()
    {
        _activeTarget = null;
        StopFiring();
    }
    
    // Checks whether the given target is within the firing range of the turret
    private bool IsWithinRange(GameObject target)
    {
        return (Vector2.Distance(transform.position, target.transform.position) <= fireRange);
    }

    // Instantiates bullets at a given interval
    private IEnumerator FireAtInterval(int interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Debug.Log("Shooting bullet now: " + Time.time);
            FireBullet();
        }
    }
    
    // NOTE: Enemies should be calling onTriggerEnter
    private void FireBullet()
    {
        if (!GameManager.instance._gameActive)
            return;
        Debug.Log("Shooting!");
        var t = transform;
        Instantiate(bulletPrefab, t.position + t.forward, t.rotation);
    }
}
