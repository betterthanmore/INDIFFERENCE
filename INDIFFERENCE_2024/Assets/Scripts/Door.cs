using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float moveDistance = 3f; // ���� �ö� �Ÿ�
    public float moveSpeed = 2f; // �̵� �ӵ�
    private Vector3 closedPosition; // ���� ��ġ
    private Vector3 openPosition; // ���� ��ġ
    private bool isOpen = false; // �� ����

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
            Debug.Log("�������� �����ϴ�."); 
        }
    }

    private IEnumerator ToggleDoor()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = isOpen ? closedPosition : openPosition; // ��ǥ ��ġ ����
        Vector3 startPosition = transform.position; // ���� ��ġ ����

        while (elapsedTime < moveDistance / moveSpeed) // �̵� �ð� ���� �ݺ�
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * moveSpeed) / moveDistance);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        transform.position = targetPosition; // ���� ��ġ ����
        isOpen = !isOpen; // �� ���� ����
    }
}