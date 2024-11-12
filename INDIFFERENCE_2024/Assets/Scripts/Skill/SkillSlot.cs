using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image slotImage;
    public Skill AssignedSkill { get; private set; }

    private Sprite defaultSprite;


    private void Awake()
    {
        defaultSprite = slotImage.sprite;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragDrop.dragIcon != null)
        {
            Sprite skillIcon = DragDrop.dragIcon.GetComponent<Image>().sprite;
            Skill skillData = DragDrop.dragIcon.GetComponent<Skill>();
            if (skillData != null)
            {
                if (IsIconAlreadyAssigned(skillIcon))
                {
                    return; 
                }
                SlotManager.Instance.HandleSkillAssignment(skillIcon, this, skillData);

                switch (transform.GetSiblingIndex())
                {
                    case 0:
                        SkillManager.Instance.SetSlotKeyMapping(KeyCode.Z, this);
                        break;
                    case 1:
                        SkillManager.Instance.SetSlotKeyMapping(KeyCode.X, this);
                        break;
                    case 2:
                        SkillManager.Instance.SetSlotKeyMapping(KeyCode.C, this);
                        break;
                }
            }
        }
    }
    private bool IsIconAlreadyAssigned(Sprite skillIcon)
    {
        return slotImage.sprite == skillIcon;
    }
    public Sprite GetSlotImage()
    {
        return slotImage.sprite;
    }
    public void SetSlot(Sprite newSprite, Skill skill)
    {
        slotImage.sprite = newSprite;
        AssignedSkill = skill;
        Debug.Log($"'{skill.skillName}'가'{name}'에 배치되었습니다.");
    }

    public void ClearSlot()
    {
        Debug.Log("클리어");
        slotImage.sprite = defaultSprite;
        AssignedSkill = null;
    }
    public void UseSkill()
    {
        AssignedSkill?.UseSkill();
    }
}