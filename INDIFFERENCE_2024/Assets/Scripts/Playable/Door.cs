using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float moveDistance = 3f; 
    public float moveSpeed = 2f;
    private Vector3 closedPosition; 
    private Vector3 openPosition; 
    private bool isOpen = false; 

    public string requiredObjectTag = "Key";

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + new Vector3(0, moveDistance, 0);
    }

    public void Interact()
    {
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
        CarryableObject currentItem = playerInventory.GetCurrentItem(); 

        if (currentItem != null && currentItem.CompareTag(requiredObjectTag))
        {
            playerInventory.DropCurrentItem(); 
            Destroy(currentItem.gameObject);
            StartCoroutine(ToggleDoor()); 
        }
        else
        {
            Debug.Log("유효한 아이템이 없습니다.");
        }
    }

    private IEnumerator ToggleDoor()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = isOpen ? closedPosition : openPosition; 
        Vector3 startPosition = transform.position; 

        while (elapsedTime < moveDistance / moveSpeed) 
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * moveSpeed) / moveDistance);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        transform.position = targetPosition; 
        isOpen = !isOpen; 
    }
}