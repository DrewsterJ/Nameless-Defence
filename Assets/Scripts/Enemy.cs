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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
