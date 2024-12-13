using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressStart : MonoBehaviour
{
    void Update()
    {
        if(Input.anyKeyDown)
        {
            this.gameObject.SetActive(false);           
        }
    }
}
