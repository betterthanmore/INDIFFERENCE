using UnityEngine;
using System.Collections;

public class SkillEffectManager : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerScript;
    private SpriteRenderer[] spriteRenderers; 
    private Rigidbody2D playerRig;
    private float transparencyDuration = 5f;

    // Ư�� ���̾� ID
    private int groundLayer; // Ground ���̾�

    void Start()
    {
        spriteRenderers = player.GetComponentsInChildren<SpriteRenderer>();
        playerRig = player.GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.NameToLayer("Ground");
        playerScript = player.GetComponent<PlayerController>();
    }

    public void StartTransparency()
    {
        playerScript.isTransparency = true;
        foreach (var renderer in spriteRenderers)
        {
            Color color = renderer.color;
            color.a = 0.5f; 
            renderer.color = color;
        }

        IgnoreAllLayersExceptGround();

        Debug.Log("����ȭ ��ų �ߵ�: �� ��� ����");
        StartCoroutine(EndTransparency());
    }

    private IEnumerator EndTransparency()
    {
        yield return new WaitForSeconds(transparencyDuration);
        playerScript.isTransparency = false;
        foreach (var renderer in spriteRenderers)
        {
            Color color = renderer.color;
            color.a = 1f;
            renderer.color = color;
        }
        ResetAllLayerCollisions();

        Debug.Log("����ȭ ��ų ����");
    }

    private void IgnoreAllLayersExceptGround()
    {
        int playerLayer = player.layer;

        for (int i = 0; i < 10; i++)
        {
            if (i != groundLayer) 
            {
                Physics2D.IgnoreLayerCollision(playerLayer, i, true);
            }
        }
    }

    private void ResetAllLayerCollisions()
    {
        int playerLayer = player.layer;

        for (int i = 0; i < 10; i++)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, i, false);
        }
    }
}