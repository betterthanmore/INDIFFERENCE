using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject breakParticle;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            PlayerInfo player = other.GetComponent<PlayerInfo>();
            if (player != null)
            {
                player.TakeDamage(50);
            }
        }
        if (breakParticle!=null)
        {
            GameObject particleInstance = Instantiate(breakParticle, transform.position, Quaternion.identity);
            Destroy(particleInstance, 2f);
        }
    }
}
