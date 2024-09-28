using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeleportable
{
    void Teleport(PlayerCotnroller player);
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
            PlayerCotnroller player = other.GetComponent<PlayerCotnroller>();
            if(teleportable != null)
            {
                teleportable.Teleport(player);
            }
        }
    }
}
