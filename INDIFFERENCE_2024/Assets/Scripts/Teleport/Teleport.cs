using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeleportable
{
    void Teleport(PlayerController player);
}

public class Teleport : MonoBehaviour
{
    private ITeleportable teleportable;

    private void Start()
    {
        teleportable = GetComponent<ITeleportable>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("텔포가능");
            PlayerController player = other.GetComponent<PlayerController>();
            if(teleportable != null)
            {
                teleportable.Teleport(player);
            }
        }
    }
}
