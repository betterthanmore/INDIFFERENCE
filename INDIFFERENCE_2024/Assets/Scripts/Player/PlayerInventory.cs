using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<CarryableObject> inventorySlots = new List<CarryableObject>();
    private CarryableObject currentItem;
    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            inventorySlots.Add(null); 
        }
        ActivateItem(0);
    }
    private void Update()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString())) 
            {
                ActivateItem(i); 
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropCurrentItem();
        }
    }

    public void AddItem(CarryableObject item)
    {
        for(int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] == null && !inventorySlots.Contains(item))
            {                                                                   //추가로 UI 창에서 획득한 아이템 칸으로 활성화
                if (currentItem != null)
                {
                    currentItem.gameObject.SetActive(false);
                }
                inventorySlots[i] = item;
                Debug.Log("아이템 추가");
                return;
            }
        }
    }

    public void ActivateItem(int index)
    {
        if (index >= 0 && index < inventorySlots.Count)
        {
            if (currentItem != null)
            {
                currentItem.gameObject.SetActive(false);
            }
            currentItem = inventorySlots[index];

            if (currentItem != null)
            {
                currentItem.gameObject.SetActive(true);
            }

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (i != index && inventorySlots[i] != null)
                {
                    inventorySlots[i].gameObject.SetActive(false); 
                }
            }
        }
    }

    public CarryableObject GetCurrentItem()
    {
        return currentItem;
    }

    public void DropCurrentItem()
    {
        if (currentItem != null)
        {
            currentItem.OnDrop();
            currentItem = null;
        }
    }
}