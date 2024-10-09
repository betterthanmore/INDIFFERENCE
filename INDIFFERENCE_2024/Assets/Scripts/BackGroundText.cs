using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundText : MonoBehaviour
{
    public Transform targetPosition; 
    public Image displayImage;
    public float triggerDistance = 1.0f;
    public float hideDistance = 2.0f;

    private bool imageDisplayed = false; //

    void Start()
    {
        displayImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!imageDisplayed && Vector3.Distance(transform.position, targetPosition.position) <= triggerDistance)
        {
            ShowImage();
        }

        if (imageDisplayed && Vector3.Distance(transform.position, targetPosition.position) > hideDistance)
        {
            HideImage();
        }
    }

    void ShowImage()
    {
        displayImage.gameObject.SetActive(true);

        imageDisplayed = true;
    }

    void HideImage()
    {
        displayImage.gameObject.SetActive(false);

        imageDisplayed = false;
    }
}
