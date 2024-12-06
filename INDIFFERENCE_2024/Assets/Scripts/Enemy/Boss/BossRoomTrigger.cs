using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    public CameraController cameraController; // CameraController 스크립트 참조
    public float showBossDuration = 3f;       // 보스 시점을 보여줄 시간

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 카메라가 보스를 보여준 뒤 다시 플레이어를 따라가도록 설정
            StartCoroutine(cameraController.ShowBossAndReturn(showBossDuration));
        }
    }
}
