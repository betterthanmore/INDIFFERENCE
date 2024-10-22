using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlinkManager : MonoBehaviour
{
    
    public TextMeshProUGUI flashingText;
    // Start is called before the first frame update
    void Start()
    {
        flashingText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(BlinkText());
    }
    public IEnumerator BlinkText()
    {
        while (true)
        {
            flashingText.text = "";
            yield return new WaitForSeconds(.5f);
            flashingText.text = "PRESS ANY KEY";
            yield return new WaitForSeconds(.5f);
        }
    }
}
