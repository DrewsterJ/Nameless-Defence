using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Public fields
    public Rigidbody2D rigidBody;
    
    // Private fields
    private Coroutine attackCoroutine;
    
    // Other labeled fields
    [Space(10)]
    [Header("Health")]
    public int minHealth;
    public int maxHealth; 
    [SerializeField] private int _health;
    
    [Space(10)]
    [Header("Attack")]
    public int attackDamage; // Damage against other game objects
    public int baseDamage; // Damage against base health bar when reaching bottom of map
    public int attackSpeed; // Seconds between attacks
    public float attackRange;
    
    [Space(10)]
    [Header("Movement")]
    public int movementSpeed;

    [Space(10)]
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
        MoveForward();
    }

    private void MoveForward()
    {
        // Source: https://discussions.unity.com/t/how-can-i-convert-a-quaternion-to-a-direction-vector/80376
        var direction = transform.rotation * Vector2.up;
        rigidBody.velocity = -direction * movementSpeed;
    }
    
    private IEnumerator MeleeAttackFrontAtInterval(int interval)
    {
        while (_health > minHealth)
        {
            yield return new WaitForSeconds(interval);
            MeleeAttackFront();
        }
    }
    
    private void MeleeAttackFront()
    {
        var facingDirection = transform.rotation * Vector2.up;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, facingDirection, attackRange);
        
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
    
    // Despawns the enemy gameobject
    private void Die()
    {
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
        var health = _health - damage;
        
        // Updates internal health and UI health bar
        SetHealth(health);

        if (health <= minHealth)
            Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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