using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSkill : MonoBehaviour, IInteractable
{
    public PlayerController player;
    public GameObject[] skillList;
    public int skillNo;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F7))
        {
            Activate(1);
            Activate(2);
            Activate(3);
            Activate(4);
            Activate(5);
        }
    }
    public void Interact()
    {
        Activate(skillNo);  
    }
    public void Activate(int skillNum)
    {
        if (skillNum >= 0 && skillNum < skillList.Length)
        {
            skillList[skillNum].SetActive(true);
            if(skillNum == 0)
            {
                player.activatedDJ = true;
            }
        }
    }
}
