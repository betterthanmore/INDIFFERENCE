using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI textNumeric;
    private PuzzleManager puzzleManager;
    private Vector3 correctPosition;

    public bool IsCorrected { private set; get; } = false;

    private int numeric;
    public int Numeric
    {
        set
        {
            numeric = value;
            textNumeric.text = numeric.ToString();
        }
        get => numeric;
    }

    public void Setup(PuzzleManager puzzleManager, int hideNumeric, int numeric)
    {
        this.puzzleManager = puzzleManager;
        textNumeric = GetComponentInChildren<TextMeshProUGUI>();

        Numeric = numeric;
        if(Numeric == hideNumeric)
        {
            GetComponent<UnityEngine.UI.Image>().enabled = false;
            textNumeric.enabled = false;
        }
    }

    public void SetCorrectPosition()
    {
        correctPosition = GetComponent<RectTransform>().localPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        puzzleManager.IsMovePiece(this);
    }
    public void OnMoveTo(Vector3 end)
    {
        StartCoroutine("MoveTo", end);
    }
    private IEnumerator MoveTo(Vector3 end)
    {
        float current = 0;
        float percent = 0;
        float moveTime = 0.1f;
        Vector3 start = GetComponent<RectTransform>().localPosition;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;

            GetComponent<RectTransform>().localPosition = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        IsCorrected = correctPosition == GetComponent<RectTransform>().localPosition ? true : false;

        puzzleManager.CanPutPuzzle();
    }
}