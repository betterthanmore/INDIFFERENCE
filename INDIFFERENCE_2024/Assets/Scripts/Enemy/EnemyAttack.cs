using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AttackPlayer();
        }
    }
    private void AttackPlayer()
    {
        Debug.Log("�÷��̾ ���� ������ �ֽ��ϴ�. �����մϴ�!");
    }
}
