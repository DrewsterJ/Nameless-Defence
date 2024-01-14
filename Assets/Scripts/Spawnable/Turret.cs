using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Turret : MonoBehaviour
{
    // Turret attributes
    public int _firingRate; // in seconds
    public int _damage; // per bullet
    public int _maxHealth;
    public int _health;
    public float _firingRange; // range the turret can shoot
    
    // Turret projectile
    public GameObject bulletPrefab;
    
    // What the turret is shooting at
    public GameObject _activeTarget;
    
    // Turret behavior
    private bool _engagingTarget;
    private bool _firing;
    
    // Offset for accurately aiming turret at enemy
    private const float _aimAngleOffset = -90f;
    
    void Start()
    {
        Debug.Assert(_firingRate > 0);
        Debug.Assert(_damage > 0);
        Debug.Assert(_maxHealth > 0);
        Debug.Assert(!_firing);
        Debug.Assert(_activeTarget == null);
        Debug.Assert(!_engagingTarget);
        Debug.Assert(_firingRange > 0);
    }
    
    void Update()
    {
        HandleActiveTargetEngagements();
    }
    
    // Method to try engaging a given target (called by the GameManager)
    public void TryEngageTarget(GameObject target)
    {
        if (IsEngagingTarget())
            return;
        
        if (IsWithinRange(target))
            EngageTarget(target);
    }

    public bool IsEngagingTarget()
    {
        return !_activeTarget.IsUnityNull() && !_activeTarget.IsDestroyed();
    }
    
    // Applies the given amount of damage to the turret
    public void TakeDamage(int dmg)
    {
        _health -= dmg;
        
        if (_health <= 0)
            KillTurret();
    }
    
    // Handles engagements with an active target
    private void HandleActiveTargetEngagements()
    {
        if (_activeTarget.IsUnityNull())
            return;

        if (_activeTarget.IsDestroyed())
        {
            _activeTarget = null;
            return;
        }

        if (IsWithinRange(_activeTarget))
            AimAtTarget(_activeTarget);
        else
            DisengageTarget(_activeTarget);
    }

    // Method to start firing at `_firingRate`
    private void StartFiring()
    {
        _firing = true;
        StartCoroutine(FireAtInterval(_firingRate));
    }
    
    // Method to stop firing
    private void StopFiring()
    {
        _firing = false;
    }
    
    // Method to aim at a given target
    private void AimAtTarget(GameObject target)
    {
        if (target == null)
            return;
        
        // Source (from RDSquare): https://discussions.unity.com/t/how-do-i-rotate-a-2d-object-to-face-another-object/187072/2
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x -transform.position.x ) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimAngleOffset));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360);
    }

    // Method to actively follow and shoot at a given target
    private void EngageTarget(GameObject target)
    {
        _activeTarget = target;
        _engagingTarget = true;
        StartFiring();
    }
    
    // Method to stop following and shooting at a given target
    private void DisengageTarget(GameObject target)
    {
        if (_activeTarget != target) 
            return;

        StopFiring();
        _activeTarget = null;
        _engagingTarget = false;
    }
    
    // Checks whether the given target is within the firing range of the turret
    private bool IsWithinRange(GameObject target)
    {
        return (Vector2.Distance(transform.position, target.transform.position) <= _firingRange);
    }

    // Instantiates bullets at a given interval
    private IEnumerator FireAtInterval(int interval)
    {
        while (IsEngagingTarget())
        {
            var activeTarget = _activeTarget;
            yield return new WaitForSeconds(interval);
            
            // Stop this coroutine if we changed targets while `WaitForSeconds(...)` was still waiting
            if (activeTarget != _activeTarget)
                break;
            
            FireBullet();
        }
    }
    
    // NOTE: Enemies should be calling onTriggerEnter
    private void FireBullet()
    {
        var t = transform;
        Instantiate(bulletPrefab, t.position + t.forward, t.rotation);
    }
    
    // Deletes the turret
    private void KillTurret()
    {
        _firing = false;
        Destroy(gameObject);
    }
}
