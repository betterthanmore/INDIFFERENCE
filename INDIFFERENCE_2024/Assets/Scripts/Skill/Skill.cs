using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string skillName; // 스킬 이름
    public Sprite skillIcon; // 스킬 아이콘
    public string skillDescription; // 스킬 설명

    public float cooldown; // 쿨타임 시간
    private float lastUsedTime = -Mathf.Infinity; // 마지막 사용 시간

    public GameObject descriptionPanel; // 설명 패널
    public TMP_Text nameText; // 이름 텍스트
    public TMP_Text descriptionText; // 설명 텍스트

    private void Start()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false); // 설명 패널 비활성화
        }
    }

    public bool IsCooldownComplete()
    {
        return Time.time >= lastUsedTime + cooldown; // 쿨타임 확인
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
        if (descriptionPanel != null && nameText != null && descriptionText != null)
        {
            nameText.text = skillName; // 이름 텍스트 설정
            descriptionText.text = skillDescription; // 설명 텍스트 설정
            descriptionPanel.SetActive(true); // 설명 패널 활성화
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false); // 설명 패널 비활성화
        }
    }
}