using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementIcon : MonoBehaviour
{
    public Image iconImage;  // 업적 아이콘 이미지
    public GameObject lockedOverlay; // 잠금 상태일 때 보여줄 오버레이
    private Achievement achievement;
    private Button iconButton;

    public void Initialize(Achievement achievement, System.Action<Achievement> onClickCallback)
    {
        this.achievement = achievement;
        // UI 초기화
        UpdateUI();
        // 버튼 클릭 시 callback 호출
        iconButton = GetComponent<Button>();
        iconButton.onClick.AddListener(() => onClickCallback(achievement));
    }

    public void UpdateUI()
    {
        // 업적 해제 여부에 따라 아이콘 UI 변경
        if (achievement.isUnlocked)
        {
            lockedOverlay.SetActive(false);
            iconImage.color = Color.white;
        }
        else
        {
            lockedOverlay.SetActive(true);
            iconImage.color = Color.gray;
        }
    }

    public string GetAchievementName()
    {
        return achievement.name;
    }
    public Sprite GetAchievementIcon()
    {
        return iconImage.sprite;
    }

    public Achievement GetAchievement()
    {
        return achievement;
    }
}