using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Skill
{
    public string skillName;      
    public Sprite skillIcon;      
    public string skillDescription; 

    public Skill(string name, Sprite icon, string description)
    {
        skillName = name;
        skillIcon = icon;
        skillDescription = description;
    }   
}