using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private EnemyMove enemyMove;
    private void Awake()
    {
        enemyMove = transform.parent.GetComponent<EnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enemyMove.isDead && collision.gameObject.CompareTag("Player"))
        {
            enemyMove.stopMove();  // 이동을 멈추고 추적을 시작
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!enemyMove.isDead && collision.gameObject.CompareTag("Player"))
        {
            Vector3 playerPosition = collision.transform.position;

            if (!enemyMove.isAttacking)
            {
                if (playerPosition.x > transform.position.x)
                {
                    enemyMove.moveRan = 3;
                }
                else if (playerPosition.x < transform.position.x)
                {
                    enemyMove.moveRan = -3;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!enemyMove.isDead && !enemyMove.isAttacking && collision.gameObject.CompareTag("Player"))
        {
            enemyMove.startMove();
        }
    }
}
