using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed = 8f;
    [SerializeField] public float gravityMultiplier = 2.5f;
    [SerializeField] public float jumpSpeed = 15f;
    [SerializeField] Rigidbody rb;
    [SerializeField] bool isGrounded = false;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;
    private Vector3 movementInput;
    private bool jumpRequested;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //get
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

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 
                                     groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore);
        MovePlayer();

        if(jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, 0f);
        }
        jumpRequested = false;

        rb.AddForce(Physics.gravity * (gravityMultiplier - 1), ForceMode.Acceleration);
    }

    void MovePlayer()
    {
        rb.linearVelocity = new Vector3(movementInput.x * speed, rb.linearVelocity.y, 0f);
    }
}
