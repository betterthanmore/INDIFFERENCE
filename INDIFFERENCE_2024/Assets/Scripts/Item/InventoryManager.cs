using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string itemName;
        public string itemDescription;
        public int maxStackSize;
        public Sprite itemIcon;
        public int currentStackSize;
        public bool isKey; 

        public void Use(PlayerInfo player)
        {
            if (isKey)
            {
                float interactionRadius = 2f;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, interactionRadius);

                foreach (var collider in colliders)
                {
                    Door door = collider.GetComponent<Door>();
                    if (door != null)
                    {
                        player.StartCoroutine(door.ToggleDoor());
                        currentStackSize--;
                        break;
                    }
                }
            }
            else
            {
                if (itemName == "영혼조각")
                {
                    player.Heal(1);
                    currentStackSize--;
                }
                else if (itemName == "생기조각")
                {
                    if(player.currentSoul<3)
                    {
                        player.FillSoul(1);
                        currentStackSize--;
                    }
                }
            }
        }
    }

    public List<Item> inventory = new List<Item>();
    public List<Item> keyItems = new List<Item>();  

    public Image[] shortcutSlots;
    public Image[] keyItemSlots;

    private PlayerInfo playerInfo;

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        UpdateShortcutUI();
        UpdateKeyItemUI();
    }
    public void AddItem(string itemName, string itemDescription, Sprite itemIcon, int maxStackSize, bool isKey)
    {
        List<Item> targetList = isKey ? keyItems : inventory;

        foreach (var item in targetList)
        {
            if (item.itemName == itemName && item.currentStackSize < item.maxStackSize)
            {
                item.currentStackSize++;
                UpdateUI(isKey);
                return;
            }
        }

        Item newItem = new Item
        {
            itemName = itemName,
            itemDescription = itemDescription,
            itemIcon = itemIcon,
            maxStackSize = maxStackSize,
            currentStackSize = 1,
            isKey = isKey
        };
        targetList.Add(newItem);

        UpdateUI(isKey);
    }
    private void UpdateUI(bool isKey)
    {
        if (isKey)
        {
            UpdateKeyItemUI();
        }
        else
        {
            UpdateShortcutUI();
        }
    }
    private void UpdateShortcutUI()
    {
        for (int i = 0; i < shortcutSlots.Length; i++)
        {
            if (i < inventory.Count && inventory[i].currentStackSize > 0)
            {
                var item = inventory[i];
                shortcutSlots[i].GetComponentInChildren<Text>().text = $"{item.currentStackSize}";
                shortcutSlots[i].GetComponent<Image>().sprite = item.itemIcon;
                shortcutSlots[i].GetComponent<Image>().enabled = true;
            }
            else
            {
                shortcutSlots[i].GetComponentInChildren<Text>().text = "";
                shortcutSlots[i].GetComponent<Image>().sprite = null;
                shortcutSlots[i].GetComponent<Image>().enabled = false;
            }
        }
    }
    private void UpdateKeyItemUI()
    {
        for (int i = 0; i < keyItemSlots.Length; i++)
        {
            if (i < keyItems.Count)
            {
                var item = keyItems[i];
                keyItemSlots[i].GetComponent<Image>().sprite = item.itemIcon;
                keyItemSlots[i].GetComponent<Image>().enabled = true;
            }
            else
            {
                keyItemSlots[i].GetComponent<Image>().sprite = null;
                keyItemSlots[i].GetComponent<Image>().enabled = false;
            }
        }
    }
    public void UseItemInSlot(int slotIndex, bool isKeyItem = false)
    {
        var targetList = isKeyItem ? keyItems : inventory;

        if (slotIndex < targetList.Count)
        {
            targetList[slotIndex].Use(playerInfo);

            if (targetList[slotIndex].currentStackSize <= 0)
            {
                targetList.RemoveAt(slotIndex);
            }
            UpdateUI(isKeyItem);
        }
    }
}