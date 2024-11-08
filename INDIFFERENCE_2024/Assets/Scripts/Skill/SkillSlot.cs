using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image slotImage;
    public void OnDrop(PointerEventData eventData)
    {
        Image dropSprite = DragDrop.dragIcon.GetComponent<Image>();
        SkillManager.Instance.AssignSkillToSlot(this, dropSprite.sprite);
    }
    public Sprite GetSlotImage()
    {
        return slotImage.sprite;
    }

    public void SetSlotImage(Sprite newSprite)
    {
        slotImage.sprite = newSprite;
    }
    public void ClearSlot()
    {
        slotImage.sprite = null;
    }   
}
