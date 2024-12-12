using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingMenu;
    public GameObject descriptionPanel;
    public GameObject skillSlots;
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
            SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_CLICK);
        }
        else if(isPaused && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
            SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_CLICK);
        }
    }

    public void Resume()
    {
        settingMenu.SetActive(false);
        descriptionPanel.SetActive(false);
        skillSlots.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void changePanel(bool isSkillPanel)
    {
        if(isSkillPanel)
        {
            skillSlots.SetActive(true);
        }
        else
        {
            skillSlots.SetActive(false);
        }
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
    public void OnChangeButton()
    {
        descriptionPanel.SetActive(false);
    }
}
