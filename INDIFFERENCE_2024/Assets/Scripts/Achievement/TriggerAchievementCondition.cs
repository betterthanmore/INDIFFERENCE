using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAchievementCondition : AchievementCondition
{
    private bool isTriggered;

    public TriggerAchievementCondition(AchievementManager manager)
    {
        this.isTriggered = false;
    }

    public void ActivateTrigger()
    {
        isTriggered = true; 
    }

    public override bool IsConditionMet()
    {
        return isTriggered;  
    }

    public override string GetConditionDescription()
    {
        return "";
    }
}