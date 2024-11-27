using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSkill : MonoBehaviour, IInteractable
{
    public List<GameObject> skillList;

    public void Interact()
    {
        Activate(0);
    }
    public void Activate(int skillNum)
    {
        skillList[skillNum].SetActive(true);
    }
}
