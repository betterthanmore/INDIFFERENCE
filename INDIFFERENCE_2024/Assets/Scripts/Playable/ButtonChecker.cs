using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    public List<PressingButton> buttons;
    public ButtonDoor connectedDoor;

    private int currentStep = 0;
    private HashSet<int> pressedButtons = new HashSet<int>(); // 비순서적 체크용

    public bool isOrderRequired = true; // true: 순서대로, false: 비순서적
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
            // 순서대로 누르는 로직
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
                Debug.Log("잘못 눌렀음");
                ResetPuzzle();
            }
        }
        else
        {
            // 비순서적 로직
            if (!pressedButtons.Contains(buttonIndex))
            {
                pressedButtons.Add(buttonIndex);
                Debug.Log($"Button {buttonIndex + 1} pressed in any order!");

                if (pressedButtons.Count == buttons.Count)
                {
                    PuzzleSolved(); // 비순서적 퍼즐 해결
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
        Debug.Log("퍼즐 초기화");
    }

    private void PuzzleSolved()
    {
        Debug.Log("퍼즐 풀림");

        if (connectedDoor != null)
        {
            connectedDoor.OpenDoor();
        }
        else
        {
            Debug.LogWarning("Door가 연결되어 있지 않습니다.");
        }

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