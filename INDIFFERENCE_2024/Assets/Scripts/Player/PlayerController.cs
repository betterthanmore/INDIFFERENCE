using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1f;
    private float input;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public float interactableRange = 2f;           
    private IInteractable interactableObj;          

    public bool canMove = true; 

    void Update()
    {
        if (canMove)
        {
            input = Input.GetAxisRaw("Horizontal");
            if (input < 0)  // ĳ���� �¿� ����
            {
                spriteRenderer.flipX = true;
            }
            else if (input > 0)
            {
                spriteRenderer.flipX = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && interactableObj != null)
        {
            interactableObj.Interact();
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;  

        rb.velocity = new Vector2(input * walkSpeed, rb.velocity.y);  // �̵� ó��
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // IInteractable �������̽��� ������ ������Ʈ�� ã��
        interactableObj = other.GetComponent<IInteractable>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // ��ȣ�ۿ� ������ ������Ʈ ������ ����� ��
        if (other.GetComponent<IInteractable>() != null)
        {
            interactableObj = null;
        }
    }
}