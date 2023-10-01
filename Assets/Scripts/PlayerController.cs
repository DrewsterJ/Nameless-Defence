using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Stores player movement inputs (WASD or Arrows)
    private Vector2 moveInput;
    
    // Stores player movement speed
    public int speed;

    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        Debug.Assert(speed != 0, "Player speed is 0");
        Debug.Assert(rb != null);
        Debug.Assert(moveInput != null);
    }
    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput.normalized * speed;
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
}
