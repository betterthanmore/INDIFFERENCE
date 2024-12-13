using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown = 2f;
    private float lastAttackTime;

    public EnemyMove enemyMove;
    private PlayerInfo playerInfo;

    private void Awake()
    {
        enemyMove = transform.parent.GetComponent<EnemyMove>();
        playerInfo = GameObject.FindWithTag("Player").GetComponent<PlayerInfo>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!enemyMove.isDead && collision.gameObject.CompareTag("Player"))
        {
            if (!enemyMove.isAttacking && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;  // ������ ���� �ð��� ���
            }
        }
    }
    private void AttackPlayer()
    {
        StartCoroutine(AttackPlayerCoroutine());
        lastAttackTime = Time.time;
    }

    private IEnumerator AttackPlayerCoroutine()
    {
        enemyMove.StartAttack();
        Debug.Log("�÷��̾ ���� ������ �ֽ��ϴ�. �����մϴ�!");

        if (playerInfo != null)
        {
            Vector2 enemyPosition = transform.position;
            playerInfo.TakeDamage(1, enemyPosition); 
        }

        yield return new WaitForSeconds(1f);

        enemyMove.EndAttack();
    }
}
