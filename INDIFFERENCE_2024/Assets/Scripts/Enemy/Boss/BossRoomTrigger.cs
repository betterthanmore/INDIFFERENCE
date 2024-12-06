using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public CameraController cameraController; // CameraController ��ũ��Ʈ ����
    public float showBossDuration = 3f;       // ���� ������ ������ �ð�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ī�޶� ������ ������ �� �ٽ� �÷��̾ ���󰡵��� ����
            StartCoroutine(cameraController.ShowBossAndReturn(showBossDuration));
        }
    }
}
