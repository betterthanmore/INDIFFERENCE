using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractable : MonoBehaviour, IInteractable
{
    private PlayerController playerController;
    public PuzzleManager puzzleManager;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void Interact()
    {
        puzzleManager.ActivatePuzzle();
        playerController.canMove = !playerController.canMove;
    }
}