using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject breakParticle;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Point"))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            PlayerInfo player = other.GetComponent<PlayerInfo>();
            if (player != null)
            {
                player.TakeDamage(50, transform.position);
            }
        }
        Destroy(this.gameObject);
        if (breakParticle!=null)
        {
            GameObject particleInstance = Instantiate(breakParticle, transform.position, Quaternion.identity);
            Destroy(particleInstance, 2f);
        }
    }
}
