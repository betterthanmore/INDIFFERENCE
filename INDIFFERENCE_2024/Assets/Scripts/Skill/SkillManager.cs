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

        Debug.Log($"{key} Ű�� {slot.name} ���Կ� ���εǾ����ϴ�.");
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
            Debug.Log("�Ҵ�� ��ų�� �����ϴ�.");
        }
    }
}