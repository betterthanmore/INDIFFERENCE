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
            if (input < 0)  // 캐릭터 좌우 반전
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

        rb.velocity = new Vector2(input * walkSpeed, rb.velocity.y);  // 이동 처리
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // IInteractable 인터페이스를 구현한 오브젝트를 찾음
        interactableObj = other.GetComponent<IInteractable>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 상호작용 가능한 오브젝트 범위를 벗어났을 때
        if (other.GetComponent<IInteractable>() != null)
        {
            interactableObj = null;
        }
    }
}