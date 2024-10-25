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
                lastAttackTime = Time.time; // ������ ���� �ð��� ������Ʈ
            }
        }
    }
    private void AttackPlayer()
    {
        enemyMove.StartAttack();
        Debug.Log("�÷��̾ ���� ������ �ֽ��ϴ�. �����մϴ�!");

        Invoke(nameof(EndAttack), 1f); // 1�� �� ���� ����
    }

    private void EndAttack()
    {
        enemyMove.EndAttack(); // ���� ����
    }
}
