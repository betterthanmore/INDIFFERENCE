using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour, IInteractable
{
    private CheckPointManager checkPointManager;

    private void Start()
    {
        checkPointManager = FindObjectOfType<CheckPointManager>();
    }
    public void Interact()
    {
        checkPointManager.UpdateCheckPoint(gameObject);
    }
}