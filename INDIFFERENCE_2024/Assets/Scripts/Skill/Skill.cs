using UnityEngine;

[System.Serializable]
public class Skill : MonoBehaviour
{
    public string skillName;        
    public Sprite skillIcon;        
    public string skillDescription; 

    public float cooldown;         
    private float lastUsedTime = -Mathf.Infinity; 

    public bool IsCooldownComplete()
    {
        return Time.time >= lastUsedTime + cooldown;
    }
    public void UseSkill()
    {
        if (IsCooldownComplete())
        {
            lastUsedTime = Time.time;
            Debug.Log($"{skillName} 스킬 사용!");
        }
        else
        {
            Debug.Log($"{skillName} 스킬이 아직 쿨타임 중입니다.");
        }
    }
}