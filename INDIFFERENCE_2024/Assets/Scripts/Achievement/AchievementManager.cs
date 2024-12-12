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
        achievements.Add(new Achievement("시작의 발걸음", "튜토리얼을 클리어 하세요.", new TriggerAchievementCondition(this)));
        achievements.Add(new Achievement("더 이상 돌아서지 않아", "B1 구역의 보스를 저지하세요.", new KillAchievementCondition(1, "보스", this)));
        achievements.Add(new Achievement("얽매이지 않는 것", "C1 구역의 유령을 처치하세요.", new KillAchievementCondition(1, "유령", this)));
        achievements.Add(new Achievement("용감함의 한걸음", "???의 구슬을 획득하세요.", new CollectItemAchievement("BraveBead")));
        achievements.Add(new Achievement("자신감을 가져", "???의 구슬을 획득하세요.", new CollectItemAchievement("ConfidenceBead")));
        achievements.Add(new Achievement("인내하고, 관철하라", "???의 구슬을 획득하세요.", new CollectItemAchievement("PatienceBead")));
        achievements.Add(new Achievement("뭔가... ", "모든 스킬을 해금", new TriggerAchievementCondition(this)));
        achievements.Add(new Achievement("이제서야 이곳이 무엇인지 알 것 같아.", "모든 지역을 해금", new TriggerAchievementCondition(this)));
    }

    public void OnEnemyKilled(string monsterType) 
    {
        foreach (Achievement achievement in achievements)
        {
            if (achievement.condition is KillAchievementCondition killCondition)
            {
                killCondition.AddKill(monsterType); 
                CheckAndUnlockAchievement(achievement.name);
            }
        }
    }
    public void CheckItemForAchievements(string itemName)
    {
        foreach (var achievement in achievements)
        {
            if (achievement.condition is CollectItemAchievement condition)
            {
                condition.CheckItem(itemName); 
                if (condition.IsConditionMet() && !achievement.isUnlocked)
                {
                    achievement.Unlock(); 
                }
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