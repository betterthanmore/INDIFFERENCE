using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShortcutSystem : MonoBehaviour
{
    public GameObject shortcutPanel;
    public Button[] shortcutSlots;
    private bool isShortcutActive = false;
    private int hoveredSlotIndex = -1;

    public GameObject descriptionPanel; // ���� �г�
    public TMP_Text nameText;           // ������ �̸� �ؽ�Ʈ
    public TMP_Text descriptionText;    // ������ ���� �ؽ�Ʈ

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        descriptionPanel.SetActive(false); // �⺻������ ���� �г� ��Ȱ��ȭ
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ActivateShortcutPanel(true);
            Time.timeScale = 0.1f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Time.timeScale = 1f;

            if (hoveredSlotIndex != -1)
            {
                inventoryManager.UseItemInSlot(hoveredSlotIndex);
                hoveredSlotIndex = -1;
            }
            ActivateShortcutPanel(false);
            HideDescription();
        }

        if (isShortcutActive)
        {
            UpdateHoveredSlot();
        }
    }

    private void ActivateShortcutPanel(bool state)
    {
        shortcutPanel.SetActive(state);
        isShortcutActive = state;
        if (!state)
        {
            hoveredSlotIndex = -1;
        }
    }

    private void UpdateHoveredSlot()
    {
        int currentHoveredSlotIndex = hoveredSlotIndex;
        hoveredSlotIndex = -1;

        for (int i = 0; i < shortcutSlots.Length; i++)
        {
            RectTransform slotRect = shortcutSlots[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(slotRect, Input.mousePosition))
            {
                hoveredSlotIndex = i;
                ShowDescription(i); // ���� ǥ��
                break;
            }
        }
        if (hoveredSlotIndex != currentHoveredSlotIndex && hoveredSlotIndex == -1)
        {
            HideDescription(); // ���Կ��� ���콺�� ����� �� ���� �����
        }
    }

    private void ShowDescription(int slotIndex)
    {
        if (slotIndex < inventoryManager.inventory.Count) // �ش� ���Կ� �������� ������ ��
        {
            var item = inventoryManager.inventory[slotIndex]; // inventory ����Ʈ���� ������ ��������
            descriptionPanel.SetActive(true);

            // �̸��� ���� �ؽ�Ʈ ����
            nameText.text = item.itemName;
            descriptionText.text = item.itemDescription; 
        }
    }
    private void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}