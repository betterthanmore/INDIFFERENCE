using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    [SerializeField]
    private Dictionary<KeyCode, SkillSlot> slotKeyMapping = new Dictionary<KeyCode, SkillSlot>();

    private void Awake()
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

    public void SetSlotKeyMapping(KeyCode key, SkillSlot slot)
    {
        slotKeyMapping[key] = slot;

        Debug.Log($"{key} 키가 {slot.name} 슬롯에 매핑되었습니다.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseSkillInSlot(KeyCode.Z);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            UseSkillInSlot(KeyCode.X);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            UseSkillInSlot(KeyCode.C);
        }
    }

    private void UseSkillInSlot(KeyCode key)
    {
        if (slotKeyMapping.TryGetValue(key, out SkillSlot slot))
        {
            slot.UseSkill();
        }
        else
        {
            Debug.Log("할당된 스킬이 없습니다.");
        }
    }
}