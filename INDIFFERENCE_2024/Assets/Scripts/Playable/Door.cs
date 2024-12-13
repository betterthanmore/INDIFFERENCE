using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject[] beeds;

    public void ToggleDoor(int beednum)
    {
        beeds[beednum].SetActive(true);
    }
}