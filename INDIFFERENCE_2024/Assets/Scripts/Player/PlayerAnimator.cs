using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void SetGrounded(bool idle)
    {
        animator.SetBool("Idle", idle);
    }

    public void SetRunning(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }

    public void SetWallSliding(bool isWallSliding)
    {
        animator.SetBool("IsWallSliding", isWallSliding);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
    }

    public void SetClimbing(bool isClimbing)
    {
        animator.SetBool("IsClimbing", isClimbing);
    }
}