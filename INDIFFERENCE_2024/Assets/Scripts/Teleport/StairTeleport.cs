using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTeleport : MonoBehaviour, ITeleportable
{
    public Transform upperFloorTarget;  // �������� �̵��� ��ġ
    public Transform lowerFloorTarget;  // �Ʒ������� �̵��� ��ġ

    public void Teleport(PlayerCotnroller player)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && upperFloorTarget != null)
        {
            player.transform.position = upperFloorTarget.position;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && lowerFloorTarget != null)
        {
            player.transform.position = lowerFloorTarget.position;
        }
    }
}