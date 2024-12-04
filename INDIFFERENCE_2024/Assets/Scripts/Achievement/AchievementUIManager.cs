using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AchievementUIManager : MonoBehaviour
{
    public GameObject achievementNotificationPanel; // 업적 알림 패널
    public TMP_Text achievementNameText; // 업적 이름 텍스트
    public Image achievementIconImage; // 업적 아이콘 이미지
    public GameObject descriptionPanel; // 업적 설명 패널
    public TMP_Text achievementDetailText; // 업적 세부 내용
    public TMP_Text achievementConditionText; // 업적 조건
    public AchievementIcon[] achievementIcons; // 에디터에서 배치된 업적 아이콘들
    public AchievementManager achievementManager;

    public float popUpDuration = 1f;
    public float displayDuration = 2f; 
    public float fadeDownDuration = 1f; 

    private RectTransform panelRectTransform;

    void Awake()
    {
        // 업적 상태 체크 및 UI 초기화
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
                icon.UpdateUI();  // 초기 UI 업데이트
                achievement.OnUnlocked += (unlockedAchievement) =>
                {
                    icon.UpdateUI();  // 업적 잠금 해제 시 UI 업데이트
                    StartCoroutine(PopUpAndFadeDown(achievement.name , icon.iconImage.sprite));
                };
            }
        }
    }

    public void DisplayAchievementDetails(Achievement achievement)
    {
        descriptionPanel.SetActive(true); // 설명 패널 활성화
        if (achievement.isUnlocked)
        {
            achievementDetailText.text = $"{achievement.name}";
            achievementConditionText.text = $"{achievement.description}\n완료!";
        }
        else
        {
            achievementDetailText.text = $"{achievement.name}\n(잠금 상태)";
            achievementConditionText.text = achievement.condition?.GetConditionDescription() ?? "조건 없음";
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