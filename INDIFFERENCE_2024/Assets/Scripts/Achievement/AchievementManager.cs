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
        achievements.Add(new Achievement("������ �߰���", "Ʃ�丮���� Ŭ���� �ϼ���.", new TriggerAchievementCondition(this)));
        achievements.Add(new Achievement("�� �̻� ���Ƽ��� �ʾ�", "B1 ������ ������ �����ϼ���.", new KillAchievementCondition(1, "����", this)));
        achievements.Add(new Achievement("�������� �ʴ� ��", "C1 ������ ������ óġ�ϼ���.", new KillAchievementCondition(1, "����", this)));
        achievements.Add(new Achievement("�밨���� �Ѱ���", "???�� ������ ȹ���ϼ���.", new CollectItemAchievement("BraveBead")));
        achievements.Add(new Achievement("�ڽŰ��� ����", "???�� ������ ȹ���ϼ���.", new CollectItemAchievement("ConfidenceBead")));
        achievements.Add(new Achievement("�γ��ϰ�, ��ö�϶�", "???�� ������ ȹ���ϼ���.", new CollectItemAchievement("PatienceBead")));
        achievements.Add(new Achievement("����... ", "��� ��ų�� �ر�", new TriggerAchievementCondition(this)));
        achievements.Add(new Achievement("�������� �̰��� �������� �� �� ����.", "��� ������ �ر�", new TriggerAchievementCondition(this)));
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