using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<GameObject> checkPoints = new List<GameObject>();
    private GameObject player;
    private GameObject currentCheckPoint;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
        if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
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
}