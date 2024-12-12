using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPoint : MonoBehaviour, IInteractable
{
    private CheckPointManager checkPointManager;
    private GameObject player;
    private PlayerInfo playerInfo;
    public GameObject textPrefab;

    private void Start()
    {
        checkPointManager = FindObjectOfType<CheckPointManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerInfo = FindObjectOfType<PlayerInfo>();
    }

    public void Interact()
    {
        if(playerInfo.currentSoul >= 1)
        {
            if (this.CompareTag("Interactable"))
            {
                checkPointManager.UpdateCheckPoint(gameObject);
                DebugSave(player.transform, "체크포인트 세이브!");
                playerInfo.UseSoul(1);
            }
        }
    }

    private void DebugSave(Transform target, string text)
    {
        Vector3 spawnPosition = new Vector3(target.position.x, target.position.y + 1f, 0f); 
        GameObject message = Instantiate(textPrefab, spawnPosition, Quaternion.identity);

        TextMeshPro tmp = message.GetComponent<TextMeshPro>();
        if (tmp != null)
        {
            tmp.text = text;
        }

        Renderer renderer = message.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = "UI";
            renderer.sortingOrder = 10;
        }

        StartCoroutine(MoveAndFade(message));
    }

    private IEnumerator MoveAndFade(GameObject message)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = message.transform.position;

        while (elapsedTime < 2f)
        {
            message.transform.position = Vector3.Lerp(startPosition, startPosition + new Vector3(0, 1, 0), elapsedTime / 2f);

            Color color = message.GetComponent<TextMeshPro>().color;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / 2f); 
            message.GetComponent<TextMeshPro>().color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(message);
    }
}