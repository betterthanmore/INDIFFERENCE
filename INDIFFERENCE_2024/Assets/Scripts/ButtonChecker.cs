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
            Debug.Log($"{buttonIndex + 1}");                            //������� �����߸� ���

            currentStep++;

            if (currentStep == buttons.Count)
            {
                PuzzleSolved();
            }
        }
        else
        {
            Debug.Log("�߸� ������");                                    //�߰��� Ʋ���� �ʱ�ȭ
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        currentStep = 0;
        Debug.Log("���� �ʱ�ȭ");
    }

    private void PuzzleSolved()
    {
        Debug.Log("���� Ǯ��");
        if (backGroundText != null && backGroundText.textTriggers.Count > 0)
        {
            var solvedTrigger = backGroundText.textTriggers[0]; 
            solvedTrigger.message = "��, ���� ������ �����̾�."; 
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