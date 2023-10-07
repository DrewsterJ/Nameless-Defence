using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player movement inputs (WASD or Arrows)
    private Vector2 moveInput;
    
    // Player movement speed
    public int speed;
    
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;
    
    // Player model sprites
    public Sprite upDown;
    public Sprite leftRight;
    public Sprite facingFrontMovingLeft;
    public Sprite facingFrontMovingRight;
    public Sprite facingBackMovingLeft;
    public Sprite facingBackMovingRight;
    private static readonly int Moving = Animator.StringToHash("Moving");

    private int faceLeftZ = 90;
    private int faceRightZ = -90;
    private int faceUpZ = 0;
    private int faceDownZ = -180;
    private int faceUpAndRight = -45;
    private int faceUpAndLeft = 45;
    private int faceDownAndLeft = 135;
    private int faceDownAndRight = -135;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Verify variables contain expected data
        Debug.Assert(speed != 0, "Player speed is 0");
        Debug.Assert(rb != null);
        Debug.Assert(moveInput != null);
    }
    
    void Update()
    { }

    private void FixedUpdate()
    {
        rb.velocity = moveInput.normalized * speed;
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        UpdatePlayerSprite();
        UpdatePlayerAnim();
    }

    private void UpdatePlayerAnim()
    {
        anim.SetBool(Moving, (moveInput != Vector2.zero));
    }

    private void UpdatePlayerSprite()
    {
        bool movingVertical = moveInput.y != 0;
        bool movingHorizontal = moveInput.x != 0;
        bool movingLeft = moveInput.x < -0.1;
        bool movingDown = moveInput.y < -0.1;
        bool moving = moveInput != Vector2.zero;

        if (!moving)
            return;

        // Moving diagonally
        if (movingVertical && movingHorizontal)
        {
            // Moving down and moving left
            if (movingDown && movingLeft)
            {
                sr.transform.rotation = Quaternion.Euler(0, 0, faceDownAndLeft);
            }
            // Moving down and moving right
            else if (movingDown && !movingLeft)
            {
                sr.transform.rotation = Quaternion.Euler(0, 0, faceDownAndRight);
            }
            // Moving up and moving left
            else if (!movingDown && movingLeft)
            {
                sr.transform.rotation = Quaternion.Euler(0, 0, faceUpAndLeft);
            }
            // Moving up and moving right
            else
            {
                sr.transform.rotation = Quaternion.Euler(0, 0, faceUpAndRight);
            }
        }
        // Moving either vertically OR horizontally
        else
        {
            if (movingVertical)
            {
                if (movingDown)
                {
                    sr.transform.rotation = Quaternion.Euler(0, 0, faceDownZ);
                }
                else
                {
                    sr.transform.rotation = Quaternion.Euler(0, 0, faceUpZ);
                }
            }
            else if (movingHorizontal)
            {
                if (movingLeft)
                {
                    sr.transform.rotation = Quaternion.Euler(0, 0, faceLeftZ);
                }
                else
                {
                    sr.transform.rotation = Quaternion.Euler(0, 0, faceRightZ);
                }
            }
        }
    }
}
