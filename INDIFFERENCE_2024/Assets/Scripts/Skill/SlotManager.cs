using UnityEngine;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour
{
    [SerializeField] private List<SkillSlot> allSlots;

    void Start()
    {
        SkillManager.Instance.RegisterSlots(allSlots);
    }

    public void AddSlot(SkillSlot newSlot)
    {
        if (!allSlots.Contains(newSlot))
        {
            allSlots.Add(newSlot);
        }
    }
}