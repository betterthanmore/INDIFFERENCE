using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBossController : MonoBehaviour
{
    public float dropSpeed = 5f;
    public float returnSpeed = 2f;
    public float dropHeight = 3f;
    public float detectionRangeX = 1f;   // 수평 감지 범위
    public float detectionRangeY = 2f;   // 수직 감지 범위
    public Transform player;

    private Vector2 initialPosition;
    private bool isDropping = false;
    private bool isReturning = false;

    private Collider2D thwompCollider;
    private Collider2D playerCollider;

    void Start()
    {
        initialPosition = transform.position;
        thwompCollider = GetComponent<Collider2D>();
        playerCollider = player.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isDropping && !isReturning) 
        {
            if (Mathf.Abs(player.position.x - transform.position.x) < detectionRangeX &&
                player.position.y < transform.position.y &&
                transform.position.y - player.position.y < detectionRangeY)
            {
                isDropping = true;
                Physics2D.IgnoreCollision(thwompCollider, playerCollider, true);
            }
        }

        if (isDropping)
        {
            Drop();
        }
        else if (isReturning)
        {
            ReturnToStart();
        }
    }

    void Drop()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition - new Vector2(0, dropHeight), dropSpeed * Time.deltaTime);

        if (transform.position.y <= initialPosition.y - dropHeight)
        {
            isDropping = false;
            isReturning = true; 
        }
    }

    void ReturnToStart()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
        {
            isReturning = false; 
            Physics2D.IgnoreCollision(thwompCollider, playerCollider, false);
        }
    }
}
