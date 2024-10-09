using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingButton : MonoBehaviour
{
    private bool isPressed = false;
    public ButtonChecker buttonChecker;
    public int buttonIndex;
    public Rigidbody2D playerRb;
    public GameObject buttonPressed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Object"))
        {
            PressButton();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ReleaseButton();
    }

    private void PressButton()
    {
        if (!isPressed)
        {
            isPressed = true;
            Debug.Log("Button Pressed!");
            if(buttonPressed != null)
            {
                buttonPressed.SetActive(true);
            }
            if(buttonChecker != null)
            {
                buttonChecker.ButtonPressed(buttonIndex);           //버튼이 눌렸을 때 ButtonPuzzle에 알림
            }
            else
            {
                return;
            }
        }
    }

    private void ReleaseButton()
    {
        if (isPressed)
        {
            isPressed = false;
            if (buttonPressed != null)
            {
                buttonPressed.SetActive(false);
            }
            Debug.Log("Button Released!");
        }
    }
}