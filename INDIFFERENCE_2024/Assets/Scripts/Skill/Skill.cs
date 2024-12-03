using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string skillName;
    public Sprite skillIcon;
    public string skillDescription;

    public float cooldown;
    private float lastUsedTime = -Mathf.Infinity;

    public GameObject descriptionPanel;
    public Text descriptionText;

    private void Start()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }

    public bool IsCooldownComplete()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public virtual void UseSkill()
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionPanel != null && descriptionText != null)
        {
            descriptionText.text = $"{skillName}\n{skillDescription}";
            descriptionPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }
}