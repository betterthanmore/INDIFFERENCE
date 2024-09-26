using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotnroller : MonoBehaviour
{
    public float walkSpeed = 1f;
    private float input;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

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
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(input * walkSpeed, rb.velocity.y);            //이동 처리
    }
}
