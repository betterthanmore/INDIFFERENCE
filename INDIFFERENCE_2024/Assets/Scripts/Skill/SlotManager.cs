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
                Debug.Log($"'{skillData.skillName}' 스킬이 이미 다른 슬롯에 배치되어 있습니다. 해당 슬롯에서 스킬을 제거합니다.");
                break;
            }
        }
        targetSlot.SetSlot(skillIcon, skillData);
    }
}