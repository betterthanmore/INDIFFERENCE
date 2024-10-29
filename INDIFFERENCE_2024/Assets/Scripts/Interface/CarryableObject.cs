using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarryable
{
    void OnPickUp(Transform player);
    void OnDrop();
}

public class CarryableObject : MonoBehaviour, ICarryable
{
    private Transform player;
    private bool isCarried = false;
    private Rigidbody2D rb;
    public PlayerController playerController;

    public float followDistance = 1f; 
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;

    private float lastInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isCarried && player != null)
        {
            Vector2 targetPosition = (Vector2)player.position + Vector2.up * 5f;

            if (playerController.input < 0)
            {
                targetPosition += Vector2.right * followDistance;
                lastInput = -1; 
            }
            else if (playerController.input > 0)
            {
                targetPosition += Vector2.left * followDistance;
                lastInput = 1; 
            }
            else if (lastInput != 0) 
            {
                if (lastInput < 0)
                {
                    targetPosition += Vector2.right * followDistance; 
                }
                else
                {
                    targetPosition += Vector2.left * followDistance; 
                }
            }

            transform.position = Vector2.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(this);
                OnPickUp(other.transform);
            }
        }
    }

    public void OnPickUp(Transform playerTransform)
    {
        player = playerTransform;
        isCarried = true;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void OnDrop()
    {
        isCarried = false;
        player = null;

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}