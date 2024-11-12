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
            Debug.Log($"{skillName} ��ų ���!");
        }
        else
        {
            Debug.Log($"{skillName} ��ų�� ���� ��Ÿ�� ���Դϴ�.");
        }
    }
}