using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startTransform;
    public Transform endTransform;
    public float speed = 2f;
    private Vector3 targetPosition;

    private PlayerController playerController;
    Rigidbody2D rb;
    Vector2 moveDirection;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        targetPosition = endTransform.position;
        DirectonCalculate();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, startTransform.position) < 0.05f)
        {
            targetPosition = endTransform.position;
            DirectonCalculate();
        }
        if (Vector2.Distance(transform.position, endTransform.position) < 0.05f)
        {
            targetPosition = startTransform.position;
            DirectonCalculate();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    void DirectonCalculate()
    {
        moveDirection = (targetPosition - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.isOn_Mp = true;
            playerController.platformRb = rb;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.isOn_Mp = false;
        }
    }
}