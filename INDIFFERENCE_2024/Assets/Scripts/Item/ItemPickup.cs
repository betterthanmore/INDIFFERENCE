using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public int maxStackSize = 10;
    public string itemDescription;
    public bool isKey;

    private AchievementManager achievementManager;

    void Start()
    {
        achievementManager = FindObjectOfType<AchievementManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.AddItem(itemName, itemDescription, itemIcon, maxStackSize ,isKey);
                achievementManager.CheckItemForAchievements(itemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("인벤토리 매니저 못 찾음");
            }
        }
    }
}
