using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCondition
{
    // 조건을 체크하는 함수
    public virtual bool IsConditionMet()
    {
        return false;
    }
}
