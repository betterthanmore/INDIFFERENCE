using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public GameObject fogPrefab;            // Fog 프리팹
    public Transform fogSpawnPoint;        // Fog 생성 위치
    public float fogFormationTime = 2f;    // Fog가 완전히 생기기까지 걸리는 시간
    public float fogDuration = 5f;         // Fog 유지 시간
    public float fogFadeTime = 2f;         // Fog가 사라지는 데 걸리는 시간

    private GameObject activeFog;          // 활성화된 Fog
    private SpriteRenderer fogRenderer;    // Fog의 SpriteRenderer
    private Rigidbody2D playerRigidbody;   // 플레이어의 Rigidbody
    private bool isPlayerInside = false;   // 플레이어가 구간 안에 있는지 여부
    private bool fogFullyFormed = false;   // Fog가 완전히 형성되었는지 여부
    private bool isFading = false;         // 안개가 사라지는 중인지 여부

    private Coroutine destroyFogCoroutine; // DestroyFogAfterDelay 코루틴 참조
    private Coroutine fadeOutFogCoroutine; // FadeOutFog 코루틴 참조

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerRigidbody = collision.GetComponent<Rigidbody2D>();

            // Fog 생성
            if (fogPrefab != null && fogSpawnPoint != null && activeFog == null)
            {
                activeFog = Instantiate(fogPrefab, fogSpawnPoint.position, Quaternion.identity);
                fogRenderer = activeFog.GetComponent<SpriteRenderer>();

                // 초기 색상 가져오기 및 알파 값 초기화
                Color initialColor = fogRenderer.color;
                fogRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f); // 알파 값 0으로 초기화

                fogFullyFormed = false; // Fog 형성 초기화
                StartCoroutine(FormFog()); // 안개 생성 시작

                // 이전 DestroyFogAfterDelay 코루틴 정리
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

            // Fog 제거
            if (activeFog != null && !isFading)
            {
                // FadeOutFog 코루틴 중복 실행 방지
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
            // 플레이어가 움직이면 Fog 제거
            if (playerRigidbody != null && playerRigidbody.velocity.magnitude > 0.1f)
            {
                if (!isFading) // 안개가 사라지는 중이 아니면
                {
                    Debug.Log("Fog가 움직임으로 인해 사라졌습니다.");

                    // FadeOutFog 코루틴 중복 실행 방지
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

            // Fog의 알파 값을 서서히 증가
            float alpha = Mathf.Lerp(0f, 1f, timer / fogFormationTime);
            fogRenderer.color = new Color(fogRenderer.color.r, fogRenderer.color.g, fogRenderer.color.b, alpha);

            yield return null;
        }

        fogFullyFormed = true;
        Debug.Log("Fog가 완전히 형성되었습니다!");
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

            // Fog의 알파 값을 서서히 감소
            float alpha = Mathf.Lerp(1f, 0f, timer / fogFadeTime);
            fogRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
        }

        if (activeFog != null)
        {
            Destroy(activeFog);
            activeFog = null; // 안개 참조 초기화
            Debug.Log("Fog가 서서히 사라졌습니다.");
        }

        isFading = false; // 상태 초기화
    }
}
