using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    public void SetIdle()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    public void SetRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    public void SetWallSliding(bool isWallSliding)
    {
        animator.SetBool("isWallSliding", isWallSliding);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool("isJumping", isJumping);
    }

    public void SetClimbing(bool isClimbing)
    {
        animator.SetBool("isClimbing", isClimbing);
    }
}