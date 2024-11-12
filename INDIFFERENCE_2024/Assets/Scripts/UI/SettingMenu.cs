using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingMenu;

    public bool isPaused = false;

    void Start()
    {
        settingMenu.SetActive(false);
    }

    void Update()
    {
        if(!isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        else if(isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Resume()
    {
        settingMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        settingMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnResumeButton()
    {
        Resume();
    }
}
