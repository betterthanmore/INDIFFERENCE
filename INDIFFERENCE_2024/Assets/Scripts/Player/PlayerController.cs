using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float pushPullSpeed = 3f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask ropeLayer;
    public SpriteRenderer spriteRenderer;
    public float climbSpeed = 3f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float input;
    private IInteractable interactableObj;
    public float interactableRange = 2f;
    public bool canMove = true;
    private bool isClimbing = false;
    public CarryableObject currentItem;
    private HingeJoint2D ropeJoint;
    private Animator animator;
    private bool isOnJumpPad = false;
    private bool isNearObject = false; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ropeJoint = GetComponent<HingeJoint2D>();
        ropeJoint.enabled = false;
    }

    private void Update()
    {
        if (canMove)
        {
            // �÷��̾� �̵� �Է� ó��
            input = Input.GetAxisRaw("Horizontal");
            
            //animator.SetFloat("Speed", Mathf.Abs(input));
            if (input < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (input > 0)
            {
                spriteRenderer.flipX = false;
            }

            // ����
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                //animator.SetTrigger("Jump");
            }

            // ��ȣ�ۿ�
            if (Input.GetKeyDown(KeyCode.F) && interactableObj != null)
            {
                interactableObj.Interact();
                //animator.SetTrigger("Interact");
            }

            //������ ������
            if (Input.GetKeyDown(KeyCode.Q) && currentItem != null)
            {
                currentItem.OnDrop();
                currentItem = null;
            }
            if (isNearObject && Input.GetKey(KeyCode.LeftShift)) 
            {
                PushOrPullObject();
            }
            else
            {
                rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);
                /*animator.SetBool("IsPushing", false);
                animator.SetBool("IsPulling", false);*/
            }
            if (Input.GetKey(KeyCode.Space) && !isClimbing)
            {
                // ������ �������� Ȯ��
                Collider2D rope = Physics2D.OverlapCircle(transform.position, 0.5f, ropeLayer);
                if (rope != null)
                {
                    float verticalInput = Input.GetAxisRaw("Vertical");
                    rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
                    AttachToRope(rope.attachedRigidbody);
                }
            }

            // �������� ��������
            if (Input.GetKeyUp(KeyCode.Space) && isClimbing)
            {
                DetachFromRope();
            }
        }

        // �÷��̾ ���� �ִ��� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private void FixedUpdate()
    {
        if (!canMove || isOnJumpPad)
        {
            return;
        }
        if (!isNearObject || !Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);
        }
    }
    void PushOrPullObject()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object"))
            {
                Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;

                // ������Ʈ �̵� ó��
                Rigidbody2D objectRb = hitCollider.GetComponent<Rigidbody2D>();
                objectRb.MovePosition(objectRb.position + direction * pushPullSpeed * Time.fixedDeltaTime);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //�����е�� �浹
        if (other.gameObject.CompareTag("JumpPad"))
        {
            isOnJumpPad = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("JumpPad"))
        {
            isOnJumpPad = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // IInteractable �������̽��� ������ ������Ʈ�� ã��
        interactableObj = other.GetComponent<IInteractable>();

        CarryableObject item = other.GetComponent<CarryableObject>();
        if(item != null)
        {
            currentItem = item;
        }

        if (other.CompareTag("Object")) 
        {
            isNearObject = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // ��ȣ�ۿ� ������ ������Ʈ ������ ����� ��
        if (other.GetComponent<IInteractable>() != null)
        {
            interactableObj = null;
        }
        if (other.CompareTag("Object")) // ��ü�� �����
        {
            isNearObject = false; // ��ü ��ó ���� ����
        }
    }

    //���� �Ŵ޸���
    private void AttachToRope(Rigidbody2D ropeSegment)
    {
        ropeJoint.enabled = true;
        ropeJoint.connectedBody = ropeSegment;
        isClimbing = true;
    }
    //�������� ��������
    private void DetachFromRope()
    {
        ropeJoint.enabled = false;
        ropeJoint.connectedBody = null;
        isClimbing = false;
    }
}