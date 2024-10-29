using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public int moveRan;         //moveRandom
    public bool isAttacking = false;

    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartCoroutine("monsterAI");
        currentHealth = maxHealth;
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

        animator.SetBool("isMoving", Mathf.Abs(rigid.velocity.x) > 0.1f);
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
        animator.SetBool("isAttacking", true);
        // 공격 애니메이션이나 로직 추가
    }

    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true; 
        animator.SetTrigger("isDead"); 
        Destroy(gameObject, 1.0f);
    }

}
