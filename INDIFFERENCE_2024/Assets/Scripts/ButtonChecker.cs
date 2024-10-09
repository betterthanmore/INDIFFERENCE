using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    public List<PressingButton> buttons; 
    private int currentStep = 0;

    private void Start()
    {
        currentStep = 0;
    }

    public void ButtonPressed(int buttonIndex)
    {
        if (currentStep < buttons.Count && buttonIndex == currentStep)
        {
            Debug.Log($"{buttonIndex + 1}");                            //순서대로 눌러야만 통과

            currentStep++;

            if (currentStep == buttons.Count)
            {
                PuzzleSolved();
            }
        }
        else
        {
            Debug.Log("잘못 눌렀음");                                    //중간에 틀리면 초기화
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        currentStep = 0;
        Debug.Log("퍼즐 초기화");
    }

    private void PuzzleSolved()
    {
        Debug.Log("퍼즐 풀림");
        ResetPuzzle();
    }
}