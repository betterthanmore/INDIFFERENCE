using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject SkillOrb;
    public Transform Target;
    public static SkillManager Instance;
    [SerializeField] private List<SkillSlot> skillSlots;
    public void AssignSkillToSlot(SkillSlot targetSlot, Sprite newSkillSprite)
    {
        foreach (var slot in skillSlots)
        {
            if (slot.GetSlotImage() == newSkillSprite)
            {
                slot.ClearSlot();
            }
        }

        targetSlot.SetSlotImage(newSkillSprite);
    }
    public void RegisterSlots(List<SkillSlot> slots)
    {
        skillSlots = slots;
    }

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
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            //currentZSkill
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            //currentXSkill
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //currentCSkill
        }
    }
}
