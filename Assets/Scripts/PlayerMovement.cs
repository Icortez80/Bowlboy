using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 8f;

    //******** jump variables
    [SerializeField] public float gravityMultiplier = 2.5f;
    [SerializeField] public float jumpSpeed = 15f;
    [SerializeField] public float fallMultiplier = 4f;
    [SerializeField] public float lowJumpMultiplier = 9f;
    private bool jumpRequested;
    private bool jumpHeld;

    //******** dash variables
    [SerializeField] public float dashSpeed = 15f;
    [SerializeField] public float dashDuration = 0.2f;
    [SerializeField] public float dashCooldown = 0.5f;
    private bool isDashing = false;
    private float dashTimeRemaining = 0f;
    private float dashDirection = 1f;
    private float dashCooldownRemaining = 0f;
    private bool dashRequested = false;

    //******** misc. player variables
    [SerializeField] Rigidbody rb;
    [SerializeField] bool isGrounded = false;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;
    private Vector3 movementInput;
    private float facingDirection = 1f;


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
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 
                                     groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore);
        //update cooldown timer
        dashCooldownRemaining -= Time.fixedDeltaTime;

        if(dashRequested && dashCooldownRemaining <= 0f && !isDashing)
        {
            dashDirection = facingDirection;
            dashTimeRemaining = dashDuration;
            isDashing = true;
            //disable built-in gravity
            rb.useGravity = false;
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

            if (jumpRequested && isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, 0f);
            }

            if (rb.linearVelocity.y <= 0)
            {
                rb.AddForce(Physics.gravity * (fallMultiplier - 1), ForceMode.Acceleration);
            }
            else if (rb.linearVelocity.y > 0 && !jumpHeld)
            {
                rb.AddForce(Physics.gravity * (lowJumpMultiplier - 1), ForceMode.Acceleration);
            }
            else
            {
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
