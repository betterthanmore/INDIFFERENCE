using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotnroller : MonoBehaviour
{
    public float walkSpeed = 1f;
    private float input;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public float interfactableRange = 2f;           //��ȣ �ۿ� ����
    private IInteractable interactableObj;          //��ȣ �ۿ� ������ ������Ʈ

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        if(input < 0)                                           //ĳ���� �¿� ����
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
        rb.velocity = new Vector2(input * walkSpeed, rb.velocity.y);            //�̵� ó��
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        interactableObj = other.GetComponent<IInteractable>();                  //���� ��ȣ�ۿ� ������ ������Ʈ �Ҵ�
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactableObj = null;                                             //���� ������ ������ �Ҵ� ������Ʈ ���ֱ�
        }
    }
}
