using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollSkill : Skill
{
    public float rollForce = 500f;
    public Animator playerAnimator; 

    public Rigidbody2D playerRigidbody;
    public PlayerController player;

    public override void UseSkill()
    {
        if (!IsCooldownComplete())
        {
            Debug.Log($"{skillName} 스킬이 아직 쿨타임 중입니다.");
            return;
        }

        if (player.isGrounded)
        {
            Debug.Log($"{skillName} 스킬 사용!");
            base.UseSkill();
            player.StartRoll();
        }
        else
        {
            Debug.Log("스킬은 땅에서만 사용할 수 있습니다.");
        }
    }
}