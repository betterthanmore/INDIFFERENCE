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
            Debug.Log($"{skillName} ��ų�� ���� ��Ÿ�� ���Դϴ�.");
            return;
        }

        if (player.isGrounded)
        {
            Debug.Log($"{skillName} ��ų ���!");
            base.UseSkill();
            player.StartRoll();
        }
        else
        {
            Debug.Log("��ų�� �������� ����� �� �ֽ��ϴ�.");
        }
    }
}