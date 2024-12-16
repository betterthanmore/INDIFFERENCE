using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemAchievement : AchievementCondition
{
    private string targetItemName; 
    private bool isCollected;       

    public CollectItemAchievement(string targetItemName)
    {
        this.targetItemName = targetItemName;
        this.isCollected = false;
    }

    public void CheckItem(string collectedItemName)
    {
        if (collectedItemName == targetItemName)
        {
            isCollected = true; 
        }
    }

    public override bool IsConditionMet()
    {
        return isCollected;
    }

    public override string GetConditionDescription()
    {
        return isCollected ? $"{targetItemName} È¹µæ ¿Ï·á!" :  $"{targetItemName}¸¦ ¼öÁýÇÏ¼¼¿ä.";
    }
}
