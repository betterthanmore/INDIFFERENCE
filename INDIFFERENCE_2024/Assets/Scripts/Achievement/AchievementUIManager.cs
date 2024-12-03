using UnityEngine;
using UnityEngine.UI;

public class AchievementUIManager : MonoBehaviour
{
    public Transform achievementsContainer;
    public GameObject achievementIconPrefab;
    public Text achievementDetailText;
    public Text achievementConditionText;

    private AchievementManager achievementManager;

    void Start()
    {
        achievementManager = FindObjectOfType<AchievementManager>();
        PopulateAchievements();
    }

    void PopulateAchievements()
    {
        foreach (Achievement achievement in achievementManager.achievements)
        {
            GameObject iconGO = Instantiate(achievementIconPrefab, achievementsContainer);
            AchievementIcon icon = iconGO.GetComponent<AchievementIcon>();
            icon.Initialize(achievement, DisplayAchievementDetails);

            achievement.OnUnlocked += (unlockedAchievement) =>
            {
                icon.UpdateUI();
            };
        }
    }

    void DisplayAchievementDetails(Achievement achievement)
    {
        if (achievement.isUnlocked)
        {
            achievementDetailText.text = $"{achievement.name}\n{achievement.description}";
            achievementConditionText.text = "";
        }
        else
        {
            achievementDetailText.text = $"{achievement.name}\n(잠금 상태)";
            achievementConditionText.text = achievement.condition?.GetConditionDescription() ?? "조건 없음";
        }
    }
}