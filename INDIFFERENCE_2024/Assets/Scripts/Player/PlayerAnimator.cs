using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerAnimator : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    private string currentAnimation;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        SetAnimation("1_IDLE", true);
    }

    // 애니메이션 전환 함수
    private void SetAnimation(string animationName, bool loop)
    {
        if (currentAnimation == animationName) return;

        skeletonAnimation.state.SetAnimation(0, animationName, loop);
        currentAnimation = animationName;

        skeletonAnimation.state.GetCurrent(0).TimeScale = 1f;
    }

    private void Update()
    {
        skeletonAnimation.skeleton.ScaleX = playerController.input < 0 ? -1 : 1;

        if (playerController.isClimbing)
        {
            //SetAnimation("Climb", true);  
            return;
        }
        else if (!playerController.isGrounded)
        {
            if (playerController.rb.velocity.y > 0)
            {
                //SetAnimation("Jump", false); 
                return;
            }
            else
            {
                //SetAnimation("Fall", true); 
                return;
            }
        }
        else
        {
            if (playerController.input != 0)
            {
                if (playerController.isRunning)
                {
                    SetAnimation("2_MOVE", true);
                    skeletonAnimation.state.GetCurrent(0).TimeScale = 5f;
                }
                else if (playerController.isCrouching)
                {
                    //SetAnimation("CrouchWalk", true);
                    return;
                }
                else
                {
                    SetAnimation("2_MOVE", true);
                    skeletonAnimation.state.GetCurrent(0).TimeScale = 2.5f;
                }
            }
            else
            {
                if (playerController.isCrouching)
                {
                    //SetAnimation("CrouchIdle", true);  
                }
                else
                {
                    SetAnimation("1_IDLE", true); 
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && playerController.interactableObj != null)
        {
            //SetAnimation("Interact", false); 
            return;
        }
    }
}