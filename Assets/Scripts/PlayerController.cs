using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player components
    public SpriteRenderer sr;
    private Rigidbody2D rb;
    public Animator anim;
    
    // Player movement inputs (WASD or Arrows)
    private Vector2 moveInput;
    
    // Player movement speed
    public int speed;
    
    // Determines which layers the player will interact with
    public LayerMask interactLayerMask;
    
    #region Rotations Quaternions Region
    // Rotation quaternions for changing the direction the player's sprite is facing
    private static readonly Quaternion facingUp = Quaternion.Euler(0, 0, 0);
    private static readonly Quaternion facingDown = Quaternion.Euler(0, 0, -180);
    private static readonly Quaternion facingLeft = Quaternion.Euler(0, 0, 90);
    private static readonly Quaternion facingRight = Quaternion.Euler(0, 0, -90);
    private static readonly Quaternion facingUpAndLeft = Quaternion.Euler(0, 0, 45);
    private static readonly Quaternion facingUpAndRight = Quaternion.Euler(0, 0, -45);
    private static readonly Quaternion facingDownAndLeft = Quaternion.Euler(0, 0, 135);
    private static readonly Quaternion facingDownAndRight = Quaternion.Euler(0, 0, -135);
    #endregion
    
    // Player hotbar containing actions and tools they can use
    public Hotbar hotbar;

    // Efficient hash for modifying animator parameters
    private static readonly int Moving = Animator.StringToHash("Moving");
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Verify variables contain expected data
        Debug.Assert(speed != 0, "Player speed is 0");
        Debug.Assert(rb != null);
        Debug.Assert(sr != null);
        Debug.Assert(anim != null);
        Debug.Assert(hotbar != null);
    }

    private void FixedUpdate()
    {
        // Change player velocity based on player input
        rb.velocity = moveInput.normalized * speed;
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        UpdatePlayerSprite();
        UpdatePlayerAnim();
    }

    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        GameManager.instance.InteractWithFocusedTile(this);
    }

    private void UpdatePlayerAnim()
    {
        anim.SetBool(Moving, (moveInput != Vector2.zero));
    }

    // Returns a Quaternion that represents a direction being faced based on a given movement vector
    private Quaternion GetFacingDirection(Vector2 movement)
    {
        bool movingVertical = movement.y != 0;
        bool movingHorizontal = movement.x != 0;
        bool movingLeft = movement.x < -0.1;
        bool movingDown = movement.y < -0.1;

        // Moving diagonally
        if (movingVertical && movingHorizontal)
        {
            // Moving south west
            if (movingDown && movingLeft)
                return facingDownAndLeft;

            // Moving south east
            if (movingDown)
                return facingDownAndRight;

            // Moving north west
            if (movingLeft)
                return facingUpAndLeft;

            // Moving north east
            return facingUpAndRight;
        }

        // Moving up/down
        if (movingVertical)
            return (movingDown) ? facingDown : facingUp;

        // Moving left/right
        return (movingLeft) ? facingLeft : facingRight;
    }

    // Update the direction that the player sprite is facing
    private void UpdatePlayerSprite()
    {
        var moving = moveInput != Vector2.zero;

        if (!moving)
            return;

        // Rotate the player's sprite towards the direction they're moving
        sr.transform.rotation = GetFacingDirection(moveInput);
    }

    private void DrawRaycastAtLookDirection()
    {
        var facingDir = moveInput.normalized;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + facingDir, Vector3.up, 0.1f, interactLayerMask);

        if (hit.collider != null)
        {
            Debug.Log("Hit an object!");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * (hit.distance + 500),
                Color.yellow);
        }
        else
        {
            Debug.Log("Did not hit an object");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * 1000,
                Color.white);
        }
    }
}
