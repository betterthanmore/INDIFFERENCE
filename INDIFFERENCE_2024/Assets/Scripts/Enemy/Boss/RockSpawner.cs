using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab;
    public float spawnHeightOffset = 10f; 

    public int numberOfRocks = 4;
    public float spawnInterval = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnRocks(other.transform));
        }
    }

    private IEnumerator SpawnRocks(Transform playerTransform)
    {
        for (int i = 0; i < numberOfRocks; i++)
        {
            // 플레이어의 위치 위쪽에 바위를 생성
            Vector3 spawnPosition = playerTransform.position + new Vector3(0, spawnHeightOffset, 0);
            Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
