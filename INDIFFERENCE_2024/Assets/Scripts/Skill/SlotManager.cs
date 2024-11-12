using UnityEngine;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private List<SkillSlot> allSlots;
    public static SlotManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void HandleSkillAssignment(Sprite skillIcon, SkillSlot targetSlot, Skill skillData)
    {
        foreach (var slot in allSlots)
        {
            if (slot.GetSlotImage() == skillIcon)
            {
                slot.ClearSlot();
                Debug.Log($"'{skillData.skillName}' ��ų�� �̹� �ٸ� ���Կ� ��ġ�Ǿ� �ֽ��ϴ�. �ش� ���Կ��� ��ų�� �����մϴ�.");
                break;
            }
        }
        targetSlot.SetSlot(skillIcon, skillData);
    }
}