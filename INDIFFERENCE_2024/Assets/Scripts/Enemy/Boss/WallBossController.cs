using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBossController : MonoBehaviour
{
    public float dropSpeed = 5f;
    public float returnSpeed = 2f;
    public float dropHeight = 3f;
    public float proximityDetectionRangeX = 1f;
    public float proximityDetectionRangeY = 7f;
    public float longRangeDetectionDistance = 10f;
    public Transform player;

    private Vector2 initialPosition;
    private bool isDropping = false;
    private bool isReturning = false;
    private bool isLongRangeDropping = false;

    private Collider2D thwompCollider;
    private Collider2D playerCollider;

    private PlayerController playerController;

    void Start()
    {
        initialPosition = transform.position;
        thwompCollider = GetComponent<Collider2D>();
        playerCollider = player.GetComponent<Collider2D>();
        playerController = player.GetComponent<PlayerController>();
    }

    void ToggleCollision(bool ignore)
    {
        Physics2D.IgnoreCollision(thwompCollider, playerCollider, ignore);
    }

    void Update()
    {
        if (!isDropping && !isReturning && !isLongRangeDropping)
        {
            // ���� ����
            if (Mathf.Abs(player.position.x - transform.position.x) < proximityDetectionRangeX &&
                transform.position.y - player.position.y > 0 &&
                transform.position.y - player.position.y <= proximityDetectionRangeY)
            {
                Debug.Log("�ٰŸ� ���� ����: �÷��̾ ���� �Ʒ��� ��ġ!");
                DropProximity();
            }

            // ���Ÿ� ����
            else if (Vector2.Distance(player.position, transform.position) < longRangeDetectionDistance)
            {
                Debug.Log("���Ÿ� ���� ����!");
                DropOnLongRange();
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

    void DropProximity()
    {
        isDropping = true;
        ToggleCollision(true);
    }

    void DropOnLongRange()
    {
        isLongRangeDropping = true;
        isDropping = true;
        ToggleCollision(true);
    }

    void Drop()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition - new Vector2(0, dropHeight), dropSpeed * Time.deltaTime);

        if (transform.position.y <= initialPosition.y - dropHeight)
        {
            isDropping = false;
            isReturning = true;

            if (isLongRangeDropping)
            {
                // ī�޶� ��鸲 ȣ��

                // �÷��̾ �������� �������� ������ ó��
                if (playerController.isGrounded)
                {
                    Debug.Log("�÷��̾ �������� �Ծ����ϴ�!");
                }
                isLongRangeDropping = false; // ���Ÿ� ��� ���� ����
            }

            ToggleCollision(false);

        }
    }

    void ReturnToStart()
    {
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
        {
            isReturning = false;
            ToggleCollision(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player hit by WallBoss!");

            // PlayerController ��ũ��Ʈ���� Die() �޼��带 ȣ���� ��� ó��
        }
    }
}

