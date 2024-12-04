using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementIcon : MonoBehaviour
{
    public Image iconImage;  // ���� ������ �̹���
    public GameObject lockedOverlay; // ��� ������ �� ������ ��������
    private Achievement achievement;
    private Button iconButton;

    public void Initialize(Achievement achievement, System.Action<Achievement> onClickCallback)
    {
        this.achievement = achievement;
        // UI �ʱ�ȭ
        UpdateUI();
        // ��ư Ŭ�� �� callback ȣ��
        iconButton = GetComponent<Button>();
        iconButton.onClick.AddListener(() => onClickCallback(achievement));
    }

    public void UpdateUI()
    {
        // ���� ���� ���ο� ���� ������ UI ����
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