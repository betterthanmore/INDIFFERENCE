using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotnroller : MonoBehaviour
{
    public float walkSpeed = 1f;
    private float input;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public float interfactableRange = 2f;           //상호 작용 범위
    private IInteractable interactableObj;          //상호 작용 가능한 오브젝트

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        if(input < 0)                                           //캐릭터 좌우 반전
        {
            spriteRenderer.flipX = true;
        }
        else if(input > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (Input.GetKeyDown(KeyCode.F) && interactableObj != null)
        {
            interactableObj.Interact();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(input * walkSpeed, rb.velocity.y);            //이동 처리
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        interactableObj = other.GetComponent<IInteractable>();                  //현재 상호작용 가능한 오브젝트 할당
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactableObj = null;                                             //범위 밖으로 나가면 할당 오브젝트 없애기
        }
    }
}
