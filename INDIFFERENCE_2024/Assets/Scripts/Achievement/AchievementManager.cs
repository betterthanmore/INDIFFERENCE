using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public string name;
    public string description;
    public bool isUnlocked;
    public AchievementCondition condition;

    public Achievement(string name, string description, AchievementCondition condition = null)
    {
        this.name = name;
        this.description = description;
        this.isUnlocked = false;
        this.condition = condition;
    }

    public void Unlock()
    {
        if (condition != null && condition.IsConditionMet())
        {
            if (!isUnlocked)
            {
                isUnlocked = true;
                Debug.Log($"Achievement Unlocked: {name}");
            }
        }
    }
}

public class AchievementManager : MonoBehaviour
{
    public List<Achievement> achievements = new List<Achievement>();
    public KillAchievementCondition killCondition;

    void Start()
    {
        killCondition = new KillAchievementCondition(1, this); 
        achievements.Add(new Achievement("First Kill", "Achieve your first kill in the game.", killCondition));
    }
    public void OnEnemyKilled()
    {
        killCondition.AddKill();
    }

    public void CheckAndUnlockAchievement(string achievementName)
    {
        Achievement achievement = achievements.Find(a => a.name == achievementName);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.Unlock();
        }
    }

    public void DisplayUnlockedAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            if (achievement.isUnlocked)
            {
                Debug.Log($"Unlocked Achievement: {achievement.name} - {achievement.description}");
            }
        }
    }
}
