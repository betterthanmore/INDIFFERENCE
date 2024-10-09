using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    public List<PressingButton> buttons; 
    private int currentStep = 0;
    private BackGroundText backGroundText;

    private void Start()
    {
        currentStep = 0;
        backGroundText = FindObjectOfType<BackGroundText>();
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
        if (backGroundText != null && backGroundText.textTriggers.Count > 0)
        {
            var solvedTrigger = backGroundText.textTriggers[0]; 
            solvedTrigger.message = "자, 이제 모험의 시작이야."; 
            solvedTrigger.displayImage.gameObject.SetActive(true);
            solvedTrigger.displayText.gameObject.SetActive(true); 

            if (solvedTrigger.textCoroutine != null)
            {
                StopCoroutine(solvedTrigger.textCoroutine);
            }
            solvedTrigger.textCoroutine = StartCoroutine(backGroundText.TypeText(solvedTrigger));
        }
        ResetPuzzle();
    }
}