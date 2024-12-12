using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPointManager : MonoBehaviour
{
    public List<GameObject> checkPoints = new List<GameObject>();
    private GameObject player;
    private GameObject currentCheckPoint;
    private GameObject restartPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        GameObject startCheckPoint = new GameObject("StartCheckPoint");
        startCheckPoint.transform.position = player.transform.position;

        UpdateCheckPoint(startCheckPoint);
        restartPosition = startCheckPoint;
    }

    public void Respawn()
    {
        if (currentCheckPoint != null)
        {
            player.transform.position = currentCheckPoint.transform.position;
        }
    }

    public void UpdateCheckPoint(GameObject newCheckPoint)
    {
        if (currentCheckPoint != null && currentCheckPoint == newCheckPoint)
        {
            return;
        }
        currentCheckPoint = newCheckPoint; 
        if (!checkPoints.Contains(newCheckPoint))
        {
            checkPoints.Add(newCheckPoint);
        }
    }

    public void ReStart()
    {
        player.transform.position = restartPosition.transform.position;
    }
}