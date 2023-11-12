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

    public GameObject activeTarget;

    public Rigidbody2D rb;
    
    // Offset for accurately aiming at a target
    private const float _aimAngleOffset = -90f;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(!rb.IsUnityNull());
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Hit by a bullet!");
            Destroy(other.gameObject);
        }
    }
}
