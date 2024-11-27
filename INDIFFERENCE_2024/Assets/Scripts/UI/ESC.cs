using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESC : MonoBehaviour
{
    public GameObject[] panels;
    public void ShowPannel(GameObject panelToActaviate)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        if(panelToActaviate != null)
        {
            panelToActaviate.SetActive(true);
        }
    }
}
