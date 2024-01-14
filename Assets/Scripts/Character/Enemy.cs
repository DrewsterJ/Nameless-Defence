using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int _maxHealth;
    public int _health;
    public int _damage;
    public int _movementSpeed;
    public int _attackSpeed;
    public float _meleeAttackRange;
    
    public bool _engagingTarget;

    public GameObject activeTarget;
    public Rigidbody2D rb;

    public FloatingHealthbar healthBar;
    
    // Offset for accurately aiming at a target
    private const float _aimAngleOffset = -90f;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(!rb.IsUnityNull());

        healthBar.SetMaxHealth(_health);
        healthBar.SetHealthAmount(_health);
        
        StartCoroutine(MeleeAttackFrontAtInterval(_attackSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        // Source: https://discussions.unity.com/t/how-can-i-convert-a-quaternion-to-a-direction-vector/80376
        var direction = transform.rotation * Vector2.up;
        rb.velocity = -direction * _movementSpeed;
    }
    
    private IEnumerator MeleeAttackFrontAtInterval(int interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            MeleeAttackFront();
        }
    }

    private void MeleeAttackFront()
    {
        var facingDirection = transform.rotation * Vector2.up;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, facingDirection, _meleeAttackRange);
        
        List<GameObject> validHits = new List<GameObject>();
        foreach (var hit in hits)
        {
            // TODO: Uncomment code once walls are implemented
            /* if (hit.collider.gameObject.CompareTag("Wall"))
            {
                  var obj = hit.collider.gameObject;
                  var wall = obj.GetComponent<Wall>();
                  wall.TakeDamage(_damage);
            }
            */
        }
    }
    
    // Deletes the enemy
    private void KillEnemy()
    {
        Destroy(gameObject);
    }

    // Method is called by others who deal damage to this enemy
    private void TakeDamage(int dmg)
    {
        _health -= dmg;
        healthBar.SetHealthAmount(_health);

        if (_health <= 0)
            KillEnemy();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            var dmg = other.GetComponent<Bullet>().damage;
            TakeDamage(dmg);
            Destroy(other.gameObject);
        }
    }
}