using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AchievementUIManager : MonoBehaviour
{
    public GameObject achievementNotificationPanel; // ���� �˸� �г�
    public TMP_Text achievementNameText; // ���� �̸� �ؽ�Ʈ
    public Image achievementIconImage; // ���� ������ �̹���
    public GameObject descriptionPanel; // ���� ���� �г�
    public TMP_Text achievementDetailText; // ���� ���� ����
    public TMP_Text achievementConditionText; // ���� ����
    public AchievementIcon[] achievementIcons; // �����Ϳ��� ��ġ�� ���� �����ܵ�
    public AchievementManager achievementManager;

    public float popUpDuration = 1f;
    public float displayDuration = 2f; 
    public float fadeDownDuration = 1f; 

    private RectTransform panelRectTransform;

    void Awake()
    {
        // ���� ���� üũ �� UI �ʱ�ȭ
        UpdateAchievementIcons();
        panelRectTransform = achievementNotificationPanel.GetComponent<RectTransform>();
        achievementNotificationPanel.SetActive(false);
    }

    public void UpdateAchievementIcons()
    {
        for (int i = 0; i < achievementIcons.Length; i++)
        {
            if (i < achievementManager.achievements.Count)
            {
                Achievement achievement = achievementManager.achievements[i];
                AchievementIcon icon = achievementIcons[i];
                icon.Initialize(achievement, DisplayAchievementDetails);
                icon.UpdateUI();  // �ʱ� UI ������Ʈ
                achievement.OnUnlocked += (unlockedAchievement) =>
                {
                    icon.UpdateUI();  // ���� ��� ���� �� UI ������Ʈ
                    StartCoroutine(PopUpAndFadeDown(achievement.name , icon.iconImage.sprite));
                };
            }
        }
    }

    public void DisplayAchievementDetails(Achievement achievement)
    {
        descriptionPanel.SetActive(true); // ���� �г� Ȱ��ȭ
        if (achievement.isUnlocked)
        {
            achievementDetailText.text = $"{achievement.name}";
            achievementConditionText.text = $"{achievement.description}\n�Ϸ�!";
        }
        else
        {
            achievementDetailText.text = $"{achievement.name}\n(��� ����)";
            achievementConditionText.text = achievement.condition?.GetConditionDescription() ?? "���� ����";
        }
    }
    private IEnumerator PopUpAndFadeDown(string achievementName, Sprite achievementIcon)
    {
        achievementNotificationPanel.SetActive(true);

        Vector2 startPos = panelRectTransform.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, startPos.y + 150);

        achievementNameText.text = achievementName;
        achievementIconImage.sprite = achievementIcon;

        float time = 0;
        while (time < popUpDuration)
        {
            panelRectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, time / popUpDuration);
            time += Time.deltaTime;
            yield return null;
        }
        panelRectTransform.anchoredPosition = endPos;

        yield return new WaitForSeconds(displayDuration);

        time = 0;
        Vector2 downPos = new Vector2(startPos.x, startPos.y);

        while (time < fadeDownDuration)
        {
            panelRectTransform.anchoredPosition = Vector2.Lerp(endPos, downPos, time / fadeDownDuration);
            time += Time.deltaTime;
            yield return null;
        }

        achievementNotificationPanel.SetActive(false);
    }
}