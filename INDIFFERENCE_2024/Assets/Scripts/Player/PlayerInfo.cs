using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public int maxSoul = 3;
    public int currentSoul;
    private PlayerController playerController;
    public CheckPointManager respawnPosition;

    // 무적 상태 관련 변수
    private bool isInvincible = false;
    public float invincibleDuration = 2f;  // 무적 지속 시간
    public float blinkInterval = 0.2f;     // 깜빡이는 간격

    public SpriteRenderer[] playerRenderer;

    // 넉백 관련 변수
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f;

    public Image[] currentHpBeeds;
    public Image[] currentSoulBeeds;
    public GameObject[] maxHpBeeds;
    public GameObject[] maxSoulBeeds;

    public GameObject gameOverPanel;

    void Start()
    {
        currentHealth = maxHealth;
        currentSoul = maxSoul;
        playerController = GetComponent<PlayerController>();
        HpUIUpdate();
        SoulUIUpdate();
    }

    // 적의 위치를 추가 인자로 받음
    public void TakeDamage(int damage, Vector2 enemyPosition)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            HpUIUpdate();
            if (currentHealth > 0)
            {
                StartCoroutine(Knockback(enemyPosition));
                StartCoroutine(BlinkAndInvincibility());
            }
            else
            {
                Die();
                gameOverPanel.SetActive(true);
                playerController.canMove = false;
            }
        }
    }

    public void Heal(int amount)
    {
        if (maxHealth < currentHpBeeds.Length)
        {
            maxHealth = Mathf.Min(maxHealth + amount, currentHpBeeds.Length);
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }
        HpUIUpdate();
    }
    public void FillSoul(int amount)
    {
        if(currentSoul < maxHealth)
        {
            currentSoul = Mathf.Min(currentSoul + amount, maxHealth);
            SoulUIUpdate();
        }
        else
        {
            return;
        }
    }
    public void UseSoul(int amout)
    {
        if(currentSoul>=1)
        {
            currentSoul--;
            SoulUIUpdate();
        }
    }
    public void Die()
    {
        respawnPosition.ReStart();
    }

    //넉백 효과 코루틴
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

    // 무적 + 깜빡임 효과 코루틴
    private IEnumerator BlinkAndInvincibility()
    {
        isInvincible = true;

        float elapsedTime = 0f;
        bool rendererEnabled = true;
        while (elapsedTime < invincibleDuration)
        {
            rendererEnabled = !rendererEnabled;
            foreach (SpriteRenderer renderer in playerRenderer)
            {
                renderer.enabled = rendererEnabled;
            }

            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }
        foreach (SpriteRenderer renderer in playerRenderer)
        {
            renderer.enabled = true;
        }
        isInvincible = false;
    }
    void HpUIUpdate()
    {
        maxHealth = Mathf.Min(maxHealth, currentHpBeeds.Length);

        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
            {
                StartCoroutine(FillHpBeed(currentHpBeeds[i], 1f));
            }
            else
            {
                StartCoroutine(FillHpBeed(currentHpBeeds[i], 0f));
            }
        }
        for (int i = 0; i < maxHpBeeds.Length; i++)
        {
            if (i < maxHealth)
            {
                Debug.Log($"Activating Max HP Beed {i}");
                maxHpBeeds[i]?.SetActive(true);
            }
        }
    }

    // 체력 구슬이 채워지는 애니메이션
    private IEnumerator FillHpBeed(Image hpBeed, float targetFill)
    {
        float currentFill = hpBeed.fillAmount;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            hpBeed.fillAmount = Mathf.Lerp(currentFill, targetFill, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hpBeed.fillAmount = targetFill;
    }

    // Soul UI 업데이트
    void SoulUIUpdate()
    {
        for (int i = 0; i < maxSoul; i++)
        {
            if (i < currentSoul)
            {
                currentSoulBeeds[i].fillAmount = 1f; 
            }
            else
            {
                currentSoulBeeds[i].fillAmount = 0f;
            }
        }
        if (maxSoul > 3)
        {
            maxSoul = 3;
        }
    }
}
