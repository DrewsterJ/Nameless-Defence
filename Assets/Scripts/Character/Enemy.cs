using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Public fields
    public Rigidbody2D rigidBody;
    public int goldReward; // amount of gold given to the player for killing this enemy
    
    // Private fields
    private Coroutine attackCoroutine;
    
    // Other labeled fields
    [Space]
    [Header("Health")]
    public int minHealth;
    public int maxHealth; 
    [SerializeField] private int _health;
    
    [Space]
    [Header("Attack")]
    public int attackDamage; // Damage against other game objects
    public int baseDamage; // Damage against base health bar when reaching bottom of map
    public int attackSpeed; // Seconds between attacks
    public float attackRange;
    
    [Space]
    [Header("Movement")]
    public int movementSpeed;

    [Space]
    [Header("UI")] 
    public FloatingHealthBar healthBar;
    
    void Start()
    {
        Debug.Assert(minHealth >= 0);
        Debug.Assert(maxHealth > 0);
        Debug.Assert(minHealth < maxHealth);
        Debug.Assert(attackDamage >= 0);
        Debug.Assert(baseDamage >= 0);
        Debug.Assert(attackSpeed >= 0);
        Debug.Assert(attackRange >= 0.0f);
        Debug.Assert(movementSpeed > 0);
        Debug.Assert(goldReward > 0);
        Debug.Assert(rigidBody != null && !rigidBody.IsUnityNull());
        Debug.Assert(healthBar != null && !healthBar.IsUnityNull());
        Debug.Assert(attackCoroutine == null || attackCoroutine.IsUnityNull());
        
        healthBar.SetMinHealth(minHealth);
        healthBar.SetMaxHealth(maxHealth);
        SetHealth(maxHealth);

        // Set the enemy to attack all the time
        attackCoroutine = StartCoroutine(MeleeAttackFrontAtInterval(attackSpeed));
    }
    
    void Update()
    {
        if (!GameManager.instance.GameActive) return;
        MoveForward();
    }

    // Causes the enemy to walk forward
    private void MoveForward()
    {
        if (!GameManager.instance.GameActive) return;
        
        // Source: https://discussions.unity.com/t/how-can-i-convert-a-quaternion-to-a-direction-vector/80376
        var direction = transform.rotation * Vector2.up;
        rigidBody.velocity = -direction * movementSpeed;
    }
    
    // Stops the enemy from moving
    public void StopMovement()
    {
        rigidBody.velocity = Vector2.zero;
    }
    
    private IEnumerator MeleeAttackFrontAtInterval(int interval)
    {
        while (_health > minHealth)
        {
            yield return new WaitForSeconds(interval);
            MeleeAttackFront();
        }
    }
    
    // Performs damage to valid targets within the enemy's attack range
    private void MeleeAttackFront()
    {
        if (!GameManager.instance.GameActive) return;
        var facingDirection = transform.rotation * Vector2.down;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, facingDirection, attackRange);
        
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                AttackWallInFront(hit.collider.gameObject.GetComponent<Wall>());
                return;
            }
        }
    }

    private void AttackWallInFront(Wall wall)
    {
        wall.TakeDamage(attackDamage);
    }
    
    // Despawns the enemy gameobject
    private void Die()
    {
        // Give the player gold for killing this enemy
        GameManager.instance.ModifyPlayerGold(goldReward);
        Destroy(gameObject);
    }

    // Sets the enemy's health to the given value and updates floating health bar with new health
    private void SetHealth(int value)
    {
        if (value < minHealth)
            value = minHealth;

        _health = value;
        healthBar.SetHealth(value);
    }

    // Method is called when the enemy receives damage
    private void TakeDamage(int damage)
    {
        if (!GameManager.instance.GameActive) return;
        var health = _health - damage;
        
        // Updates internal health and UI health bar
        SetHealth(health);

        if (health <= minHealth)
            Die();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.instance.GameActive) return;
        if (other.CompareTag("Bullet"))
        {
            var bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet.damage);
            bullet.HandleHit();
        }
        else if (other.CompareTag("BaseBoundaryTile"))
        {
            GameManager.instance.DamageBase(baseDamage);
            Destroy(gameObject);
        }
    }
}