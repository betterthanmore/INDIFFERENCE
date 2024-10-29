using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public enum DamageType
    {
        Player, 
        Enemy   
    }

    public DamageType damageType; 
    public GameObject breakParticle;
    private Rigidbody2D rb;
    public float damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Point"))
        {
            return;
        }

        // ������Ʈ�� �Ʒ��� ������ ���� ó�� (velocity.y < 0)
        if (rb.velocity.y < 0)
        {
            //�浹 �ӵ� �� ������ �̿��� � ������ ���
            float velocity = rb.velocity.magnitude;
            float mass = rb.mass;
            damage = 0.2f * mass * velocity;  

            if (damageType == DamageType.Player)
            {
                if (collision.collider.CompareTag("Player"))
                {
                    PlayerInfo player = collision.collider.GetComponent<PlayerInfo>();
                    if (player != null)
                    {
                        player.TakeDamage((int)damage, transform.position);
                    }
                }
                Destroy(this.gameObject);
            }
            if (damageType == DamageType.Enemy && collision.collider.CompareTag("Enemy"))
            {
                EnemyMove enemy = collision.collider.GetComponent<EnemyMove>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)damage);
                }
            }

            if (breakParticle != null)
            {
                GameObject particleInstance = Instantiate(breakParticle, transform.position, Quaternion.identity);
                Destroy(particleInstance, 2f);
            }
        }
    }
}