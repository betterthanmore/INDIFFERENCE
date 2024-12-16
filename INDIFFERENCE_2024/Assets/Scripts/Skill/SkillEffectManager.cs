using UnityEngine;
using System.Collections;

public class SkillEffectManager : MonoBehaviour
{
    public GameObject player;
    public GameObject wings;
    private PlayerController playerScript;
    public SpriteRenderer[] spriteRenderers;
    private Rigidbody2D playerRig;
    private float transparencyDuration = 5f;
    public float glideDuration = 5f;
    public float maxGlideSpeed = 10f;
    public float glideAcceleration = 0.1f;
    public float glideDeceleration = 0.05f;
    private float originalSpeed;
    private bool isGliding = false;
    private float glideEndTime;
    private float currentGlideSpeed;

    // Ư�� ���̾� ID
    private int groundLayer; // Ground ���̾�

    void Start()
    {
        playerRig = player.GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.NameToLayer("Ground");
        playerScript = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (isGliding)
        {
            HandleGlideMovement();

            if (Time.time >= glideEndTime || playerScript.isGrounded)
            {
                StopGliding();
            }
        }
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

    public void StartGliding()
    {
        wings.SetActive(true);
        isGliding = true;
        originalSpeed = playerScript.moveSpeed;
        playerScript.moveSpeed *= 3f; // �۶��̵� �߿��� �ӵ��� ������Ŵ

        glideEndTime = Time.time + glideDuration;
    }

    private void StopGliding()
    {
        wings.SetActive(false);
        isGliding = false;
        playerScript.moveSpeed = originalSpeed;
        currentGlideSpeed = 0f; 
    }

    private void HandleGlideMovement()
    {
        float horizontalInput = playerScript.input;

        if (Mathf.Abs(horizontalInput) > 0.1f) // ����
        {
            AccelerateGlide(horizontalInput);
        }
        else // ����
        {
            DecelerateGlide();
        }

        // ���� �ӵ��� currentGlideSpeed��, ���� �ӵ��� ���� �߷¿� ���� �ӵ� ����
        playerRig.velocity = new Vector2(playerScript.moveSpeed* horizontalInput + currentGlideSpeed, -1.0f);
    }

    private void AccelerateGlide(float horizontalInput)
    {
        if (horizontalInput > 0) // ���������� ����
        {
            currentGlideSpeed = Mathf.Min(currentGlideSpeed + glideAcceleration, maxGlideSpeed);
        }
        else if (horizontalInput < 0) // �������� ����
        {
            currentGlideSpeed = Mathf.Max(currentGlideSpeed - glideAcceleration, -maxGlideSpeed);
        }
    }

    private void DecelerateGlide()
    {
        if (Mathf.Abs(currentGlideSpeed) > 0)
        {
            // ���� (���� �ӵ��� 0�� �����������)
            currentGlideSpeed = Mathf.MoveTowards(currentGlideSpeed, 0f, glideDeceleration);
        }
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