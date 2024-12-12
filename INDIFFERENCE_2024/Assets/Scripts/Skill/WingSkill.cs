using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderSkill : Skill
{
    private SkillEffectManager skillEffectManager;
    public override void UseSkill()
    {
        if (skillEffectManager == null)
        {
            skillEffectManager = FindObjectOfType<SkillEffectManager>();

            if (skillEffectManager == null)
            {
                Debug.LogError("SkillEffectManager�� ���� �����ϴ�.");
                return;
            }
        }

        if (IsCooldownComplete())
        {
            base.UseSkill();
            skillEffectManager.StartGliding();
        }
    }
}