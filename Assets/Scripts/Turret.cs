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
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFiring()
    {
        firing = true;
        StartCoroutine(FireAtInterval(firingRate));
    }
    
    public void StopFiring()
    {
        firing = false;
    }
    
    // Applies the given amount of damage to the turret
    public void TakeDamage(int dmg)
    {
        int newHealth = health - dmg;
        health = (newHealth < 0) ? 0 : newHealth;
        
        if (health == 0)
            KillTurret();
    }
    
    // Checks whether the given target is within the firingRange of the turret
    public bool IsTargetWithinRange(GameObject target)
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
