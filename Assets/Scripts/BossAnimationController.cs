using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void PlayDialSpin()
    {
        animator.SetTrigger("DialSpin");
    }

    public void PlayBoomerang()
    {
        animator.SetTrigger("Boomerang");
    }

    public void PlayGraphiteTop()
    {
        animator.SetTrigger("GraphiteTop");
    }

    public void PlayGraphiteBottom()
    {
        animator.SetTrigger("GraphiteBottom");
    }

    public void PlayNibBlast()
    {
        animator.SetTrigger("NibBlast");
    }

    public void PlayDefeat()
    {
        animator.SetTrigger("Defeat");
    }

    public void PlayIdle()
    {
        animator.Play("Idle", 0, 0f);
    }
}