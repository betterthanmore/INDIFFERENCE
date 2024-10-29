using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerInfo : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private int maxSoul = 3;
    private int currentSoul;
    private PlayerController playerController;
    public CheckPointManager respawnPosition;

    // ���� ���� ���� ����
    private bool isInvincible = false;
    public float invincibleDuration = 2f;  // ���� ���� �ð�
    public float blinkInterval = 0.2f;     // �����̴� ����

    private SkeletonAnimation skeletonAnimation;
    private MeshRenderer meshRenderer;

    // �˹� ���� ����
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentSoul = maxSoul;

        playerController = GetComponent<PlayerController>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // ���� ��ġ�� �߰� ���ڷ� ����
    public void TakeDamage(int damage, Vector2 enemyPosition)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;

            if (currentHealth > 0)
            {
                StartCoroutine(Knockback(enemyPosition));
                StartCoroutine(BlinkAndInvincibility());

            }
            else
            {
                //��� ó��
                UpdateSoul(-1);
                currentHealth = maxHealth;
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void UpdateSoul(int soul)
    {
        currentSoul += soul;
        if (currentSoul <= 0)
        {
            respawnPosition.ReStart();
        }
        else
        {
            //�÷��̾� ��ȥ ���� ó��
        }
    }

    //�˹� ȿ�� �ڷ�ƾ
    private IEnumerator Knockback(Vector2 enemyPosition)
    {
        float timer = 0;
        playerController.canMove = false;


        Vector2 knockbackDirection = (transform.position.x > enemyPosition.x) ? Vector2.right : Vector2.left;

        while (timer < knockbackDuration)
        {
            playerController.rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, playerController.rb.velocity.y);
            timer += Time.deltaTime;
            yield return null;
        }

        playerController.canMove = true;
    }

    //���� + ������ ȿ�� �ڷ�ƾ
    private IEnumerator BlinkAndInvincibility()
    {
        isInvincible = true;

        float elapsedTime = 0f;
        bool rendererEnabled = true;

        while (elapsedTime < invincibleDuration)
        {
            rendererEnabled = !rendererEnabled;
            meshRenderer.enabled = rendererEnabled;

            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        meshRenderer.enabled = true;
        isInvincible = false;
    }
}