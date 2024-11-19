using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    public List<PressingButton> buttons;
    public ButtonDoor connectedDoor;

    private int currentStep = 0;
    private HashSet<int> pressedButtons = new HashSet<int>(); // ������� üũ��

    public bool isOrderRequired = true; // true: �������, false: �������
    private BackGroundText backGroundText;

    private void Start()
    {
        currentStep = 0;
        backGroundText = FindObjectOfType<BackGroundText>();
    }

    public void ButtonPressed(int buttonIndex)
    {
        if (isOrderRequired)
        {
            // ������� ������ ����
            if (currentStep < buttons.Count && buttonIndex == currentStep)
            {
                Debug.Log($"{buttonIndex + 1}");
                currentStep++;

                if (currentStep == buttons.Count)
                {
                    PuzzleSolved();
                }
            }
            else
            {
                Debug.Log("�߸� ������");
                ResetPuzzle();
            }
        }
        else
        {
            // ������� ����
            if (!pressedButtons.Contains(buttonIndex))
            {
                pressedButtons.Add(buttonIndex);
                Debug.Log($"Button {buttonIndex + 1} pressed in any order!");

                if (pressedButtons.Count == buttons.Count)
                {
                    PuzzleSolved(); // ������� ���� �ذ�
                }
            }
        }
    }

    public void ButtonReleased(int buttonIndex)
    {
        if (!isOrderRequired && pressedButtons.Contains(buttonIndex))
        {
            pressedButtons.Remove(buttonIndex);
            Debug.Log($"Button {buttonIndex + 1} released!");
        }
    }

    private void ResetPuzzle()
    {
        if (isOrderRequired)
        {
            currentStep = 0;
        }
        else
        {
            pressedButtons.Clear();
        }
        Debug.Log("���� �ʱ�ȭ");
    }

    private void PuzzleSolved()
    {
        Debug.Log("���� Ǯ��");

        if (connectedDoor != null)
        {
            connectedDoor.OpenDoor();
        }
        else
        {
            Debug.LogWarning("Door�� ����Ǿ� ���� �ʽ��ϴ�.");
        }

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