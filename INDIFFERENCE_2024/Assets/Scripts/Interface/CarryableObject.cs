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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isCarried && player != null)
        {
            Vector2 direction;

            if(playerController.input < 0)
            {
                direction = Vector2.right;
            }
            else
            {
                direction = Vector2.left;
            }
            Vector2 targetPosition = (Vector2)player.position + direction * followDistance;

            transform.position = Vector2.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickUp(other.transform);
        }
    }

    // ¹°Ã¼¸¦ ÁÖ¿ò
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

    // ¶³¾î¶ß¸²
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