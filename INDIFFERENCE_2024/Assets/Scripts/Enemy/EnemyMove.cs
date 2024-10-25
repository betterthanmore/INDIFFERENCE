using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public int moveRan;         //moveRandom
    public bool isAttacking = false; 


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        StartCoroutine("monsterAI");
    }
    void Update()
    {
        if (rigid.velocity.x > 0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        //rigid.velocity = new Vector2(moveRan, rigid.velocity.y);
        if (!isAttacking)
        {
            Vector2 frontVec = new Vector2(rigid.position.x + moveRan * 0.2f, rigid.position.y - 1.0f); // 발밑으로 Ray 쏘기
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

            // 바닥이 있을 때만 이동
            if (rayHit.collider != null)
            {
                rigid.velocity = new Vector2(moveRan, rigid.velocity.y);
            }
        }
    }

    IEnumerator monsterAI()
    {
        moveRan = Random.Range(-1, 2);   // -1<= ranNum <2
        yield return new WaitForSeconds(3f);
        StartCoroutine("monsterAI");
    }

    public void startMove()
    {
        StartCoroutine("monsterAI");
    }

    public void stopMove()
    {
        StopCoroutine("monsterAI");
    }

    public void StartAttack()
    {
        isAttacking = true;
        // 공격 애니메이션이나 로직 추가
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
