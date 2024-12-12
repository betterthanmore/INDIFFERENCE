using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private CheckPointManager checkPointManager;
    private PlayerInfo playerinfo;

    private void Start()
    {
        checkPointManager = FindObjectOfType<CheckPointManager>();
        playerinfo = FindObjectOfType<PlayerInfo>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkPointManager.Respawn();
            playerinfo.TakeDamage(1,this.gameObject.transform.position);
        }
        else if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
