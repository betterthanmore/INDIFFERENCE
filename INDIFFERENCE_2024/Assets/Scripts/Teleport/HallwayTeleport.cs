using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayTeleport : MonoBehaviour, ITeleportable
{
    public Transform nextHallway;  

    public void Teleport(PlayerController player)
    {
        if (nextHallway != null)
        {
            player.transform.position = nextHallway.position;                //이동 지점에 도달시 자동 이동
        }
    }
}
