using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public int moveRan;         //moveRandom
    

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

        Vector2 frontVec = new Vector2(rigid.position.x + moveRan * 0.2f, rigid.position.y - 1.0f); // πﬂπÿ¿∏∑Œ Ray ΩÓ±‚
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        // πŸ¥⁄¿Ã ¿÷¿ª ∂ß∏∏ ¿Ãµø
        if (rayHit.collider != null)
        {
            rigid.velocity = new Vector2(moveRan, rigid.velocity.y);
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

}
