using UnityEngine;

public class ReCallSkill : Skill
{
    private CheckPointManager checkPoint;

    private void Start()
    {
        checkPoint = FindObjectOfType<CheckPointManager>();
    }
    public override void UseSkill()
    {
        if (IsCooldownComplete())
        {
            base.UseSkill();
            ReCall();
        }
    }

    private void ReCall()
    {
        checkPoint.Respawn();
    }
}
