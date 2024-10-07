using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find player
        if (collision.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<EnemyMove>().stopMove();
            Vector3 playerPosition = collision.transform.position;
            if (playerPosition.x > transform.position.x)
            {
                transform.parent.GetComponent<EnemyMove>().moveRan = 3;     // speed up
            }
            else if (playerPosition.x < transform.position.x)
            {
                transform.parent.GetComponent<EnemyMove>().moveRan = -3;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            transform.parent.GetComponent<EnemyMove>().startMove();
    }
}
