using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public bool isPuzzleActive = false;
    [SerializeField] private GameObject puzzlePrefab;
    [SerializeField] private Transform puzzleParnet;
    public GameObject puzzle;

    private List<PuzzlePiece> pieceList;

    private Vector2Int puzzleSize = new Vector2Int(4, 4);
    private float neighborPieceDistance = 204;

    public Vector3 EmptyPiecePosition { set; get; }

    private IEnumerator Start()
    {
        pieceList = new List<PuzzlePiece>();

        SpawnPuzzles();
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(puzzleParnet.GetComponent<RectTransform>());

        yield return new WaitForEndOfFrame();

        pieceList.ForEach(x => x.SetCorrectPosition());
        StartCoroutine("SufflePuzzle");
    }
    public void ActivatePuzzle()
    {
        isPuzzleActive = !isPuzzleActive;
        if(isPuzzleActive)
        {
            puzzle.SetActive(true);
        }
        if (!isPuzzleActive)
        {
            puzzle.SetActive(false);
        }
    }
    private void SpawnPuzzles()
    {
        for(int y = 0; y < puzzleSize.y; ++ y)
        {
            for (int x = 0; x< puzzleSize.x; ++ x)
            {
                GameObject clone = Instantiate(puzzlePrefab, puzzleParnet);
                PuzzlePiece piece = clone.GetComponent<PuzzlePiece>();

                piece.Setup(this,puzzleSize.x * puzzleSize.y, y * puzzleSize.x + x + 1);

                pieceList.Add(piece);
            }
        }
    }
    private IEnumerator SufflePuzzle()
    {
        float current = 0;
        float percent = 0;
        float time = 1.5f;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            int index = Random.Range(0, puzzleSize.x * puzzleSize.y);
            pieceList[index].transform.SetAsLastSibling();

            yield return null;
        }
        EmptyPiecePosition = pieceList[pieceList.Count - 1].GetComponent<RectTransform>().localPosition;
    }

    public void IsMovePiece(PuzzlePiece piece)
    {
        if(Vector3.Distance(EmptyPiecePosition, piece.GetComponent<RectTransform>().localPosition) == neighborPieceDistance)
        {
            Vector3 targetPosition = EmptyPiecePosition;
            EmptyPiecePosition = piece.GetComponent<RectTransform>().localPosition;

            piece.OnMoveTo(targetPosition);
        }
    }
    public void CanPutPuzzle()      //완성된 퍼즐에 조각을 끼울 기능 추가
    {
        List<PuzzlePiece> pieces = pieceList.FindAll(x => x.IsCorrected == true);

        if(pieces.Count == puzzleSize.x * puzzleSize.y - 1)
        {
            Debug.Log("PuzzleClear");
            //기능 추가
        }
    }
}