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

    public GameObject activeTarget;

    public Rigidbody2D rb;
    
    // Offset for accurately aiming at a target
    private const float _aimAngleOffset = -90f;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(!rb.IsUnityNull());

        StartCoroutine(MeleeAttackFrontAtInterval(_attackSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        FollowActiveTarget();
    }

    private void FollowActiveTarget()
    {
        if (activeTarget.IsUnityNull())
            return;

        if (activeTarget.IsDestroyed())
        {
            activeTarget = null;
            return;
        }

        AimAtTarget(activeTarget);
        MoveForward();
    }

    private void MoveForward()
    {
        // Source: https://discussions.unity.com/t/how-can-i-convert-a-quaternion-to-a-direction-vector/80376
        var direction = transform.rotation * Vector2.up;
        rb.velocity = direction * _movementSpeed;
    }
    
    private void AimAtTarget(GameObject target)
    {
        if (target.IsUnityNull())
            return;
        
        // Source (from RDSquare): https://discussions.unity.com/t/how-do-i-rotate-a-2d-object-to-face-another-object/187072/2
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x -transform.position.x ) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + _aimAngleOffset));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360);
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
        Debug.Log("Attacking front!");
        var facingDirection = transform.rotation * Vector2.up;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, facingDirection, _meleeAttackRange);
        
        
        List<GameObject> validHits = new List<GameObject>();
        foreach (var hit in hits)
        {
            
            if (hit.collider.gameObject.CompareTag("Turret"))
            {
                var obj = hit.collider.gameObject;
                var turret = obj.GetComponent<Turret>();
                turret.TakeDamage(_damage);
            }
            else if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Hit player");
                Debug.Log("Distance from player: " + Vector2.Distance(transform.position, hit.collider.transform.position));
                var obj = hit.collider.gameObject;
                var player = obj.GetComponent<PlayerController>();
                player.TakeDamage(_damage);
            }
        }
    }
    
    private void KillEnemy()
    {
        enabled = false;
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Hit by a bullet!");
            Destroy(other.gameObject);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Turret"))
        {
            activeTarget = other.gameObject;
            MeleeAttackFront(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            activeTarget = other.gameObject;
            MeleeAttackFront(other.gameObject);
        }
    }*/
}
