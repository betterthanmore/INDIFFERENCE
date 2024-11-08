using UnityEngine;
using UnityEngine.UI;

public class ShortcutSystem : MonoBehaviour
{
    public GameObject shortcutPanel;         
    public Button[] shortcutSlots;           
    private bool isShortcutActive = false;   
    private int hoveredSlotIndex = -1;

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
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
                break;
            }
        }
        if (hoveredSlotIndex != currentHoveredSlotIndex)
        {
            if (hoveredSlotIndex != -1)
            {
                Debug.Log($"���� ���콺�� ��ġ�� ���� �ε���: {hoveredSlotIndex}");
            }
            else
            {
                Debug.Log("���콺�� ���� ���� ���� �ʽ��ϴ�.");
            }
        }
    }
}