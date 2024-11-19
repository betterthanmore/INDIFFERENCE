using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + new Vector3(0, moveDistance, 0);
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            StartCoroutine(ToggleDoor());
        }
    }

    private IEnumerator ToggleDoor()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = isOpen ? closedPosition : openPosition;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveDistance / moveSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime * moveSpeed) / moveDistance);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isOpen = !isOpen;
    }
}
