using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAchievementCondition : AchievementCondition
{
    private int requiredKills;
    private int currentKills;

    public KillAchievementCondition(int requiredKills, AchievementManager manager)
    {
        this.requiredKills = requiredKills;
        this.currentKills = 0;
    }

    public void AddKill()
    {
        currentKills++;
    }

    public override bool IsConditionMet()
    {
        return currentKills >= requiredKills;
    }

    public override string GetConditionDescription()
    {
        return $"Àû Ã³Ä¡ {currentKills}/{requiredKills}";
    }
}