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
                Debug.LogError("SkillEffectManager가 씬에 없습니다.");
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