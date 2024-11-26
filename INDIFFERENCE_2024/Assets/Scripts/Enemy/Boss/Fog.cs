using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public GameObject fogPrefab;            // Fog ������
    public Transform fogSpawnPoint;        // Fog ���� ��ġ
    public float fogFormationTime = 2f;    // Fog�� ������ �������� �ɸ��� �ð�
    public float fogDuration = 5f;         // Fog ���� �ð�
    public float fogFadeTime = 2f;         // Fog�� ������� �� �ɸ��� �ð�

    private GameObject activeFog;          // Ȱ��ȭ�� Fog
    private SpriteRenderer fogRenderer;    // Fog�� SpriteRenderer
    private Rigidbody2D playerRigidbody;   // �÷��̾��� Rigidbody
    private bool isPlayerInside = false;   // �÷��̾ ���� �ȿ� �ִ��� ����
    private bool fogFullyFormed = false;   // Fog�� ������ �����Ǿ����� ����
    private bool isFading = false;         // �Ȱ��� ������� ������ ����

    private Coroutine destroyFogCoroutine; // DestroyFogAfterDelay �ڷ�ƾ ����
    private Coroutine fadeOutFogCoroutine; // FadeOutFog �ڷ�ƾ ����

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerRigidbody = collision.GetComponent<Rigidbody2D>();

            // Fog ����
            if (fogPrefab != null && fogSpawnPoint != null && activeFog == null)
            {
                activeFog = Instantiate(fogPrefab, fogSpawnPoint.position, Quaternion.identity);
                fogRenderer = activeFog.GetComponent<SpriteRenderer>();

                // �ʱ� ���� �������� �� ���� �� �ʱ�ȭ
                Color initialColor = fogRenderer.color;
                fogRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f); // ���� �� 0���� �ʱ�ȭ

                fogFullyFormed = false; // Fog ���� �ʱ�ȭ
                StartCoroutine(FormFog()); // �Ȱ� ���� ����

                // ���� DestroyFogAfterDelay �ڷ�ƾ ����
                if (destroyFogCoroutine != null)
                    StopCoroutine(destroyFogCoroutine);

                destroyFogCoroutine = StartCoroutine(DestroyFogAfterDelay());
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;

            // Fog ����
            if (activeFog != null && !isFading)
            {
                // FadeOutFog �ڷ�ƾ �ߺ� ���� ����
                if (fadeOutFogCoroutine != null)
                    StopCoroutine(fadeOutFogCoroutine);

                fadeOutFogCoroutine = StartCoroutine(FadeOutFog());
            }
        }
    }

    void Update()
    {
        if (isPlayerInside && fogFullyFormed && activeFog != null)
        {
            // �÷��̾ �����̸� Fog ����
            if (playerRigidbody != null && playerRigidbody.velocity.magnitude > 0.1f)
            {
                if (!isFading) // �Ȱ��� ������� ���� �ƴϸ�
                {
                    Debug.Log("Fog�� ���������� ���� ��������ϴ�.");

                    // FadeOutFog �ڷ�ƾ �ߺ� ���� ����
                    if (fadeOutFogCoroutine != null)
                        StopCoroutine(fadeOutFogCoroutine);

                    fadeOutFogCoroutine = StartCoroutine(FadeOutFog());
                }
            }
        }
    }

    private IEnumerator FormFog()
    {
        float timer = 0f;

        while (timer < fogFormationTime)
        {
            timer += Time.deltaTime;

            // Fog�� ���� ���� ������ ����
            float alpha = Mathf.Lerp(0f, 1f, timer / fogFormationTime);
            fogRenderer.color = new Color(fogRenderer.color.r, fogRenderer.color.g, fogRenderer.color.b, alpha);

            yield return null;
        }

        fogFullyFormed = true;
        Debug.Log("Fog�� ������ �����Ǿ����ϴ�!");
    }

    private IEnumerator DestroyFogAfterDelay()
    {
        yield return new WaitForSeconds(fogFormationTime + fogDuration);

        if (activeFog != null && !isFading)
        {
            fadeOutFogCoroutine = StartCoroutine(FadeOutFog());
        }
    }

    private IEnumerator FadeOutFog()
    {
        isFading = true;

        float timer = 0f;
        Color initialColor = fogRenderer.color;

        while (timer < fogFadeTime)
        {
            timer += Time.deltaTime;

            // Fog�� ���� ���� ������ ����
            float alpha = Mathf.Lerp(1f, 0f, timer / fogFadeTime);
            fogRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
        }

        if (activeFog != null)
        {
            Destroy(activeFog);
            activeFog = null; // �Ȱ� ���� �ʱ�ȭ
            Debug.Log("Fog�� ������ ��������ϴ�.");
        }

        isFading = false; // ���� �ʱ�ȭ
    }
}
