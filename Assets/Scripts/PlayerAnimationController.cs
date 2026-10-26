using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerInputReader inputReader;
    private PlayerMovement movement;
    private PlayerHealth health;

    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        bool isMoving = Mathf.Abs(inputReader.MoveAim.x) > 0.1f;
        bool isShooting = inputReader.ShootHeld;

        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsShooting", isShooting);
        animator.SetBool("IsGrounded", movement.IsGrounded);
        animator.SetBool("IsDashing", movement.IsDashing);
        animator.SetBool("IsSuper", movement.IsSupering);
        animator.SetFloat("VerticalSpeed", movement.VerticalSpeed);

        animator.SetBool("IsDefeated", health.IsDefeated);

        if (movement.FacingDirection < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.FacingDirection > 0f)
        {
            spriteRenderer.flipX = false;
        }
    }
}