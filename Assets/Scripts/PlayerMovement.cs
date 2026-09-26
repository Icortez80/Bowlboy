using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 8f;

    //******** jump variables
    [SerializeField] public float gravityMultiplier = 2.5f;
    [SerializeField] public float fullJumpSpeed = 12f;
    [SerializeField] public float shortJumpSpeed = 10f;
    [SerializeField] public float fallMultiplier = 4f;
    [SerializeField] public float shortHopTime = 0.15f;
    [SerializeField] public float coyoteTime = 0.15f;
    private bool jumpRequested;
    private bool jumpHeld;
    private bool jumpDecisionPending = false;
    private float jumpTimer = 0f;
    private float coyoteTimer = 0f;

    //******** dash variables
    [SerializeField] public float dashSpeed = 15f;
    [SerializeField] public float dashDuration = 0.2f;
    [SerializeField] public float dashCooldown = 0.5f;
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private float dashDirection = 1f;
    private float dashCooldownRemaining = 0f;
    private bool dashRequested = false;
    private float facingDirection = 1f;

    //******** misc. player variables
    [SerializeField] Rigidbody rb;
    [SerializeField] bool isGrounded = false;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;
    private Vector3 movementInput;
    private RaycastHit groundHit;

    OneWayPlatform platform = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //get keyboard input for horizontal movement
        if (Keyboard.current.aKey.isPressed)
        {
            //update speed to move left
            movementInput = new Vector3(-1, 0f, 0f);

        }else if (Keyboard.current.dKey.isPressed)
        {
            //update speed to move right
            movementInput = new Vector3(1, 0f, 0f);
        }
        else
        {
            //update speed to stop (zero)
            movementInput = new Vector3(0, 0f, 0f);
        }

        if (movementInput.x != 0f && !isDashing)
        {
            facingDirection = movementInput.x;
        }

        //jumping logic
        jumpHeld = Keyboard.current.spaceKey.isPressed;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }

        //dashing logic
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            dashRequested = true;
        }
    }

    private void FixedUpdate()
    {
        //grounded check
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, out groundHit, 
                                     groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore);
        //update cooldown timer
        dashCooldownRemaining -= Time.fixedDeltaTime;
        if (isGrounded) 
        { 
            coyoteTimer = coyoteTime; 
        } 
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        if(dashRequested && dashCooldownRemaining <= 0f && !isDashing)
        {
            dashDirection = facingDirection;
            dashTimeRemaining = dashDuration;
            isDashing = true;
            //disable built-in gravity
            rb.useGravity = false;
            jumpDecisionPending = false;
        }
        if (isDashing)
        {
            rb.linearVelocity = new Vector3(dashDirection * dashSpeed, 0f, 0f);
            dashTimeRemaining -= Time.fixedDeltaTime;
            if(dashTimeRemaining <= 0f)
            {
                //end dash
                isDashing = false;
                //restore built-in gravity
                rb.useGravity = true;
                dashCooldownRemaining = dashCooldown;
            }
        }
        else
        {
            MovePlayer();

            //add jump velocity to player
            if (jumpRequested && (isGrounded || coyoteTimer > 0))
            {
                if (groundHit.collider != null)
                {
                    platform = groundHit.collider.GetComponent<OneWayPlatform>();
                }

                if (Keyboard.current.sKey.isPressed && platform != null)
                {
                    platform.dropThrough();
                    jumpDecisionPending = false;
                    coyoteTimer = 0f;
                }
                else
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, shortJumpSpeed, 0f);
                    jumpTimer = shortHopTime;
                    jumpDecisionPending = true;
                }
            }
            else if (jumpDecisionPending)
            {
                if (!Keyboard.current.spaceKey.isPressed)
                {
                    jumpDecisionPending = false;
                }
                else
                {
                    jumpTimer -= Time.fixedDeltaTime;
                    if(jumpTimer <= 0)
                    {
                        rb.linearVelocity = new Vector3(rb.linearVelocity.x, fullJumpSpeed, 0f);
                        jumpDecisionPending = false;
                    }
                }
            }

            if (rb.linearVelocity.y <= 0)
            {
                //this affects the downward portion of the jump
                rb.AddForce(Physics.gravity * (fallMultiplier - 1), ForceMode.Acceleration);
            }
            else
            {
                //this affects the rising portion of the jump
                rb.AddForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);
            }
        }

        jumpRequested = false;
        dashRequested = false;
    }

    void MovePlayer()
    {
        rb.linearVelocity = new Vector3(movementInput.x * speed, rb.linearVelocity.y, 0f);
    }
}
