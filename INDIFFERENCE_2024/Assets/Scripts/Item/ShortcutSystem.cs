using UnityEngine;
using UnityEngine.UI;

public class ShortcutSystem : MonoBehaviour
{
    public GameObject shortcutPanel;
    public Button[] shortcutSlots;
    private bool isShortcutActive = false;
    private int hoveredSlotIndex = -1;

    public GameObject descriptionPanel; // 설명 패널
    public Text descriptionText;        // 설명 텍스트

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        descriptionPanel.SetActive(false); // 기본적으로 설명 패널 비활성화
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
                ShowDescription(i); // 설명 표시
                break;
            }
        }
        if (hoveredSlotIndex != currentHoveredSlotIndex && hoveredSlotIndex == -1)
        {
            HideDescription(); // 슬롯에서 마우스가 벗어났을 때 설명 숨기기
        }
    }

    private void ShowDescription(int slotIndex)
    {
        if (slotIndex < inventoryManager.inventory.Count) // 해당 슬롯에 아이템이 존재할 때
        {
            var item = inventoryManager.inventory[slotIndex]; // inventory 리스트에서 아이템 가져오기
            descriptionPanel.SetActive(true);
            descriptionText.text = $"{item.itemName}\n{item.itemDescription}"; // 아이템 이름과 설명 표시
            Vector3 mousePosition = Input.mousePosition;
            descriptionPanel.transform.position = mousePosition + new Vector3(10f, 10f, 0f); // 마우스 옆에 위치
        }
    }

    private void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}