using System.Collections;
using System.Collections.Generic;
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
    
    // What the turret shoots
    public GameObject bulletPrefab;
    
    // What the turret is shooting at
    private GameObject _activeTarget;
    
    // Info about turret behavior
    private bool _engagingTarget;
    private bool _firing;
    
    // Offset for accurately aiming turret at enemy
    private const float _aimAngleOffset = -90f;
    
    // Start is called before the first frame update
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
    
    // Update is called once per frame
    void Update()
    {
        HandleActiveTargetEngagements();
    }
    
    // Attempt to engage a given target (called by the GameManager)
    public void TryEngageTarget(GameObject target)
    {
        if (IsWithinRange(target))
            EngageTarget(target);
    }

    public bool IsEngagingTarget()
    {
        return _engagingTarget;
    }
    
    // Applies the given amount of damage to the turret
    public void TakeDamage(int dmg)
    {
        int newHealth = _health - dmg;
        _health = (newHealth < 0) ? 0 : newHealth;
        
        if (_health == 0)
            KillTurret();
    }
    
    // Handles engagements with an active target
    private void HandleActiveTargetEngagements()
    {
        if (_activeTarget == null)
            return;

        if (IsWithinRange(_activeTarget))
            AimAtTarget(_activeTarget);
        else
            DisengageTarget(_activeTarget);
    }

    private void StartFiring()
    {
        _firing = true;
        StartCoroutine(FireAtInterval(_firingRate));
    }
    
    private void StopFiring()
    {
        _firing = false;
    }

    private void AimAtTarget(GameObject target)
    {
        if (target == null)
            return;
        
        // Source (from RDSquare): https://discussions.unity.com/t/how-do-i-rotate-a-2d-object-to-face-another-object/187072/2
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x -transform.position.x ) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimAngleOffset));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360);
    }

    private void EngageTarget(GameObject target)
    {
        _activeTarget = target;
        _engagingTarget = true;
        StartFiring();
    }
    
    private void DisengageTarget(GameObject target)
    {
        if (_activeTarget != target) 
            return;

        StopFiring();
        _activeTarget = null;
        _engagingTarget = false;
    }
    
    // Checks whether the given target is within the firingRange of the turret
    private bool IsWithinRange(GameObject target)
    {
        return (Vector2.Distance(transform.position, target.transform.position) <= _firingRange);
    }

    // Instantiates bullets at the given interval
    private IEnumerator FireAtInterval(int interval)
    {
        while (_firing)
        {
            yield return new WaitForSeconds(interval);
            FireBullet();
        }
    }
    
    // TODO: Enemies should be calling onTriggerEnter
    private void FireBullet()
    {
        var t = transform;
        Instantiate(bulletPrefab, t.position + t.forward, t.rotation);
    }
    
    // Called to delete the turret
    private void KillTurret()
    {
        _firing = false;
        enabled = false;
        Destroy(this);
    }
}
