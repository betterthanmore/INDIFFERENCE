using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string skillName; // ��ų �̸�
    public Sprite skillIcon; // ��ų ������
    public string skillDescription; // ��ų ����

    public float cooldown; // ��Ÿ�� �ð�
    private float lastUsedTime = -Mathf.Infinity; // ������ ��� �ð�

    public GameObject descriptionPanel; // ���� �г�
    public TMP_Text nameText; // �̸� �ؽ�Ʈ
    public TMP_Text descriptionText; // ���� �ؽ�Ʈ

    private void Start()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false); // ���� �г� ��Ȱ��ȭ
        }
    }

    public bool IsCooldownComplete()
    {
        return Time.time >= lastUsedTime + cooldown; // ��Ÿ�� Ȯ��
    }

    public virtual void UseSkill()
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionPanel != null && nameText != null && descriptionText != null)
        {
            nameText.text = skillName; // �̸� �ؽ�Ʈ ����
            descriptionText.text = skillDescription; // ���� �ؽ�Ʈ ����
            descriptionPanel.SetActive(true); // ���� �г� Ȱ��ȭ
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false); // ���� �г� ��Ȱ��ȭ
        }
    }
}