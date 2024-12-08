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

        public void Use(PlayerInfo player)
        {
            if (itemName == "Potion")
            {
                player.Heal(50);
                currentStackSize--;
            }
            else if (itemName == "Mana Potion")
            {
                Debug.Log("플레이어의 마나가 회복되었습니다.");
                currentStackSize--;
            }
            else if (itemName == "Big Health Potion")
            {
                player.Heal(100);
                currentStackSize--;
            }
            else if (itemName == "Key")
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
        }
    }

    public List<Item> inventory = new List<Item>();
    public Image[] shortcutSlots;
    private PlayerInfo playerInfo;

    void Start()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        UpdateShortcutUI();
    }

    public void AddItem(string itemName, string itemDescription, Sprite itemIcon, int maxStackSize)
    {
        foreach (var item in inventory)
        {
            if (item.itemName == itemName && item.currentStackSize < item.maxStackSize)
            {
                item.currentStackSize++;
                UpdateShortcutUI();
                return;
            }
        }

        Item newItem = new Item
        {
            itemName = itemName,
            itemDescription = itemDescription,
            itemIcon = itemIcon,
            maxStackSize = maxStackSize,
            currentStackSize = 1
        };
        inventory.Add(newItem);

        for (int i = 0; i < shortcutSlots.Length; i++)
        {
            if (shortcutSlots[i].GetComponentInChildren<Text>().text == "")
            {
                AssignToShortcutSlot(newItem, i);
                break;
            }
        }

        UpdateShortcutUI();
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
            }
        }
    }

    private void AssignToShortcutSlot(Item item, int slotIndex)
    {
        shortcutSlots[slotIndex].GetComponentInChildren<Text>().text = $"{item.currentStackSize}";
        shortcutSlots[slotIndex].GetComponent<Image>().sprite = item.itemIcon;
        shortcutSlots[slotIndex].GetComponent<Image>().enabled = true;
    }

    public void UseItemInSlot(int slotIndex)
    {
        if (slotIndex < inventory.Count)
        {
            inventory[slotIndex].Use(playerInfo);

            if (inventory[slotIndex].currentStackSize <= 0)
            {
                inventory.RemoveAt(slotIndex);
            }

            UpdateShortcutUI();
        }
    }
}