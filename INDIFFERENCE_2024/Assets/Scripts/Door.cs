using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float moveDistance = 3f; // 문이 올라갈 거리
    public float moveSpeed = 2f; // 이동 속도
    private Vector3 closedPosition; // 닫힌 위치
    private Vector3 openPosition; // 열린 위치
    private bool isOpen = false; // 문 상태

    private void Start()
    {
        closedPosition = transform.position; 
        openPosition = closedPosition + new Vector3(0, moveDistance, 0); 
    }

    public void Interact()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player.currentItem != null) 
        {
            Destroy(player.currentItem.gameObject);
            StartCoroutine(ToggleDoor());
        }
        else
        {
            Debug.Log("아이템이 없습니다."); 
        }
    }

    private IEnumerator ToggleDoor()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = isOpen ? closedPosition : openPosition; // 목표 위치 설정
        Vector3 startPosition = transform.position; // 시작 위치 설정

        while (elapsedTime < moveDistance / moveSpeed) // 이동 시간 동안 반복
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * moveSpeed) / moveDistance);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        transform.position = targetPosition; // 최종 위치 설정
        isOpen = !isOpen; // 문 상태 반전
    }
}