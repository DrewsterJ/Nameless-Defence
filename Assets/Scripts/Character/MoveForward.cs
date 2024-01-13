using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(rb != null);
        Debug.Assert(speed > 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.up * (speed * Time.deltaTime));
    }
}