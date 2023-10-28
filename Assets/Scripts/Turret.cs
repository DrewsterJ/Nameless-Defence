using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public int firingRate; // in seconds
    public int damage; // per bullet
    public int maxHealth;
    public int health;
    public float firingRange; // range the turret can shoot
    public GameObject bulletPrefab;
    
    private bool firing;
    private GameObject activeTarget;
    
    // Start is called before the first frame update
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        HandleActiveTargetEngagements();
    }
    
    // Attempt to engage a given target (called by the GameManager)
    public void TryEngageTarget(GameObject target)
    {
        Debug.Assert(target);

        if (IsWithinRange(target))
            EngageTarget(target);
    }
    
    // Applies the given amount of damage to the turret
    public void TakeDamage(int dmg)
    {
        int newHealth = health - dmg;
        health = (newHealth < 0) ? 0 : newHealth;
        
        if (health == 0)
            KillTurret();
    }
    
    // Handles engagements with an active target
    private void HandleActiveTargetEngagements()
    {
        if (activeTarget == null)
            return;

        if (IsWithinRange(activeTarget))
            AimAtTarget(activeTarget);
        else
            DisengageTarget(activeTarget);
    }

    private void StartFiring()
    {
        firing = true;
        StartCoroutine(FireAtInterval(firingRate));
    }
    
    private void StopFiring()
    {
        firing = false;
    }

    private void AimAtTarget(GameObject target)
    {
        if (target == null)
            return;
        
        transform.LookAt(target.transform);
    }

    private void EngageTarget(GameObject target)
    {
        activeTarget = target;
        StartFiring();
    }
    
    private void DisengageTarget(GameObject target)
    {
        if (activeTarget != target) 
            return;

        StopFiring();
        activeTarget = null;
    }
    
    // Checks whether the given target is within the firingRange of the turret
    private bool IsWithinRange(GameObject target)
    {
        return (Vector2.Distance(transform.position, target.transform.position) <= firingRange);
    }

    // Instantiates bullets at the given interval
    private IEnumerator FireAtInterval(int interval)
    {
        while (firing)
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
        firing = false;
        enabled = false;
        Destroy(this);
    }
}
