using UnityEngine;
using UnityEngine.UI;

public class AchievementIcon : MonoBehaviour
{
    public Image iconImage; // 업적 아이콘 이미지
    public GameObject lockedOverlay; // 잠금 상태일 때 보여줄 오버레이
    private Achievement achievement;

    public void Initialize(Achievement achievement, System.Action<Achievement> onHoverCallback)
    {
        this.achievement = achievement;

        // UI 초기화
        UpdateUI();

        // 아이콘에 마우스 올릴 때 이벤트 등록
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => onHoverCallback(achievement));
    }

    public void UpdateUI()
    {
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
}