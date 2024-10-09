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
        ResetPuzzle();
    }
}