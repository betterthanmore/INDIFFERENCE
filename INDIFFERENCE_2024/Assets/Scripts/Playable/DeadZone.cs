using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private PlayerInfo playerinfo;

    private void Start()
    {
        playerinfo = FindObjectOfType<PlayerInfo>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinfo.TakeDamage(1,this.gameObject.transform.position);
        }
        else if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
