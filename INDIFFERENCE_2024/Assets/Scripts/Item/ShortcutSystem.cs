using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShortcutSystem : MonoBehaviour
{
    public GameObject shortcutPanel;
    public Button[] shortcutSlots;
    public Button[] keyItemSlots;
    private bool isShortcutActive = false;
    private int hoveredSlotIndex = -1;
    private bool isKeyItemHovered = false;

    public GameObject descriptionPanel;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        descriptionPanel.SetActive(false);
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
                inventoryManager.UseItemInSlot(hoveredSlotIndex, isKeyItemHovered);
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
            isKeyItemHovered = false;
        }
    }

    private void UpdateHoveredSlot()
    {
        int currentHoveredSlotIndex = hoveredSlotIndex;
        hoveredSlotIndex = -1;
        isKeyItemHovered = false;

        for (int i = 0; i < shortcutSlots.Length; i++)
        {
            RectTransform slotRect = shortcutSlots[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(slotRect, Input.mousePosition))
            {
                hoveredSlotIndex = i;
                ShowDescription(i, false);
                return;
            }
        }

        for (int i = 0; i < keyItemSlots.Length; i++)
        {
            RectTransform slotRect = keyItemSlots[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(slotRect, Input.mousePosition))
            {
                hoveredSlotIndex = i;
                isKeyItemHovered = true;
                ShowDescription(i, true);
                return;
            }
        }

        if (hoveredSlotIndex == currentHoveredSlotIndex)
        {
            return;
        }
    }

    private void ShowDescription(int index, bool isKey)
    {
        if (isKey)
        {
            if (index >= 0 && index < inventoryManager.keyItems.Count)
            {
                var item = inventoryManager.keyItems[index];
                descriptionPanel.SetActive(true);
                nameText.text = item.itemName;
                descriptionText.text = item.itemDescription;
            }
            else
            {
                HideDescription();
            }
        }
        else
        {
            if (index >= 0 && index < inventoryManager.inventory.Count)
            {
                var item = inventoryManager.inventory[index];
                descriptionPanel.SetActive(true);

                nameText.text = item.itemName;
                descriptionText.text = item.itemDescription;
            }
            else
            {
                HideDescription();
            }
        }
    }

    private void HideDescription()
    {
        descriptionPanel.SetActive(false);
    }
}