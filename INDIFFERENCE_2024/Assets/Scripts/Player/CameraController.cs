using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    public float smoothTime;

    public Vector3 positionOffest;
    public Vector2 xLimit;
    public Vector2 yLimit;

    public Transform bossViewPoint;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position+positionOffest;
        targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y), Mathf.Clamp(targetPosition.y, yLimit.x, yLimit.y), -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public IEnumerator ShowBossAndReturn(float delay)
    {
        // 1. 카메라를 보스 시점으로 이동
        target = bossViewPoint;

        // 2. 지정된 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 3. 다시 플레이어를 따라가도록 복구
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
