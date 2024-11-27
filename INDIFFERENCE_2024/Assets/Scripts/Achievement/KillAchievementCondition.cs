using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAchievementCondition : AchievementCondition
{
    private int requiredKills;
    private int currentKills;
    private AchievementManager achievementManager; 

    public KillAchievementCondition(int requiredKills, AchievementManager achievementManager)
    {
        this.requiredKills = requiredKills;
        this.currentKills = 0;
        this.achievementManager = achievementManager;
    }

    public void AddKill()
    {
        currentKills++;
        Debug.Log($"Current Kills: {currentKills}");
        if (currentKills >= requiredKills)
        {
            Debug.Log("Kill achievement condition met!");
            achievementManager.CheckAndUnlockAchievement("First Kill");
        }
    }

    public override bool IsConditionMet()
    {
        return currentKills >= requiredKills;
    }
}