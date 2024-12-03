using UnityEngine;
using UnityEngine.UI;

public class AchievementIcon : MonoBehaviour
{
    public Image iconImage; // ���� ������ �̹���
    public GameObject lockedOverlay; // ��� ������ �� ������ ��������
    private Achievement achievement;

    public void Initialize(Achievement achievement, System.Action<Achievement> onHoverCallback)
    {
        this.achievement = achievement;

        // UI �ʱ�ȭ
        UpdateUI();

        // �����ܿ� ���콺 �ø� �� �̺�Ʈ ���
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