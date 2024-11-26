using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseZone : MonoBehaviour
{
    public float reverseDuration = 10f;
    private bool hasActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasActivated && collision.CompareTag("Player"))
        {
            hasActivated = true;
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                StartCoroutine(ReverseControlsTemporarily(player));
            }

        }
    }

    private System.Collections.IEnumerator ReverseControlsTemporarily(PlayerController player)
    {
        player.InvertControls(true);
        yield return new WaitForSeconds(reverseDuration);
        player.InvertControls(false);
    }
}
