using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAchievementCondition : AchievementCondition
{
    private int requiredKills;
    private int currentKills;
    private string monsterType;

    public KillAchievementCondition(int requiredKills, string monsterType, AchievementManager manager)
    {
        this.requiredKills = requiredKills;
        this.currentKills = 0;
        this.monsterType = monsterType; 
    }


    public void AddKill(string killedMonsterType)
    {
        if (killedMonsterType == monsterType)
        {
            currentKills++;
        }
    }

    public override bool IsConditionMet()
    {
        return currentKills >= requiredKills;
    }

    public override string GetConditionDescription()
    {
        return $"???ÀÇ ±¸½½ {currentKills}/{requiredKills}";
    }
}