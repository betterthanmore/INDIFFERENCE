using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float attackCooldown = 2f;
    private float lastAttackTime;

    public EnemyMove enemyMove;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time; // 마지막 공격 시간을 업데이트
            }
        }
    }
    private void AttackPlayer()
    {
        enemyMove.StartAttack();
        Debug.Log("플레이어가 공격 범위에 있습니다. 공격합니다!");

        Invoke(nameof(EndAttack), 1f); // 1초 후 공격 종료
    }

    private void EndAttack()
    {
        enemyMove.EndAttack(); // 공격 종료
    }
}
