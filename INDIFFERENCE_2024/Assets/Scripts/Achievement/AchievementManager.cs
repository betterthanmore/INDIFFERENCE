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

    public event System.Action<Achievement> OnUnlocked;

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
                OnUnlocked?.Invoke(this);
            }
        }
    }
}

public class AchievementManager : MonoBehaviour
{
    public List<Achievement> achievements = new List<Achievement>();

    void Start()
    {
        achievements.Add(new Achievement("First Kill", "첫 번째 적을 처치하세요.", new KillAchievementCondition(1, this)));
        achievements.Add(new Achievement("Five Kills", "적을 5명 처치하세요.", new KillAchievementCondition(5, this)));
    }

    public void OnEnemyKilled()
    {
        foreach (Achievement achievement in achievements)
        {
            if (achievement.condition is KillAchievementCondition killCondition)
            {
                killCondition.AddKill();
                CheckAndUnlockAchievement(achievement.name);
            }
        }
    }

    public void CheckAndUnlockAchievement(string achievementName)
    {
        Achievement achievement = achievements.Find(a => a.name == achievementName);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.Unlock();
        }
    }
}