using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private int maxSoul = 3;
    private int currentSoul;
    private PlayerController playerController;
    private CheckPointManager respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentSoul = maxSoul;

        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth > 0)
        {
            //플레이어 피격 애니메이션 재생          플레이어 컨트롤러로 넘겨서 재생시킬 것 같음.
        }
        else
        {
            //플레이어 사망 애니메이션 끝난 후 처리되게
            UpdateSoul(-1);
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
            respawnPosition.Respawn();
        }
    }
}
