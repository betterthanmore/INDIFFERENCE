using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AchievementCondition
{
    public abstract bool IsConditionMet();
    public abstract string GetConditionDescription(); 
}
