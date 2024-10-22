using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeedMultiplier = 1.5f;
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

    public bool isOn_Mp = false;
    public Rigidbody2D platformRb;

    public float fallMultiplier = 1.5f;
    private float maxFallSpeed = 19.6f;

    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;
    private bool isCrouching = false;
    private bool isRunning = false;
    public float crouchSpeedMultiplier = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ropeJoint = GetComponent<HingeJoint2D>();
        ropeJoint.enabled = false;
        boxCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = boxCollider.size;
    }

    private void Update()
    {
        if (canMove)
        {
            // 플레이어 이동 입력 처리
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
            //달리기
            if(isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
                //animator.SetBool("IsRungging", true);
            }
            else if (isGrounded && Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;
                //animator.SetBool("IsRungging", false);
            }
            //앉기
            if (isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = true;
                boxCollider.size = new Vector2(boxCollider.size.x, originalColliderSize.y / 2f);
                //animator.SetBool("IsCrouching", true);
            }
            else if (isGrounded && Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCrouching = false;
                boxCollider.size = originalColliderSize;
                //animator.SetBool("IsCrouching", false);
            }
            // 점프
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                //animator.SetTrigger("Jump");
            }

            // 상호작용
            if (Input.GetKeyDown(KeyCode.F) && interactableObj != null)
            {
                interactableObj.Interact();
                //animator.SetTrigger("Interact");
            }

            //아이템 떨구기
            if (Input.GetKeyDown(KeyCode.Q) && currentItem != null)
            {
                currentItem.OnDrop();
                currentItem = null;
            }
            if (isNearObject && Input.GetKey(KeyCode.C)) 
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
                // 로프와 접촉한지 확인
                Collider2D rope = Physics2D.OverlapCircle(transform.position, 0.5f, ropeLayer);
                if (rope != null)
                {
                    float verticalInput = Input.GetAxisRaw("Vertical");
                    rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
                    AttachToRope(rope.attachedRigidbody);
                }
            }

            // 로프에서 떨어지기
            if (Input.GetKeyUp(KeyCode.Space) && isClimbing)
            {
                DetachFromRope();
            }
        }

        // 플레이어가 땅에 있는지 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isGrounded && rb.velocity.y < 0)
        {
            float adjustedFallMultiplier = Mathf.SmoothStep(1f, fallMultiplier, -rb.velocity.y / 5f);
            adjustedFallMultiplier = Mathf.Clamp(adjustedFallMultiplier, 1f, fallMultiplier);

            rb.velocity += Vector2.up * Physics2D.gravity.y * (adjustedFallMultiplier - 1) * Time.fixedDeltaTime;

            if (rb.velocity.y < -maxFallSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!canMove || isOnJumpPad)
        {
            return;
        }

        float currentSpeed = moveSpeed;

        if (isRunning)
        {
            currentSpeed *= runSpeedMultiplier;
        }
        else if (isCrouching)
        {
            currentSpeed *= crouchSpeedMultiplier;
        }

        if (isNearObject && Input.GetKey(KeyCode.C))
        {
            PushOrPullObject();
        }
        else
        {
            rb.velocity = new Vector2(input * currentSpeed, rb.velocity.y);
        }

        if (isOn_Mp)
        {
            rb.velocity = new Vector2(platformRb.velocity.x + input * currentSpeed, rb.velocity.y);
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

                // 오브젝트 이동 처리
                Rigidbody2D objectRb = hitCollider.GetComponent<Rigidbody2D>();
                objectRb.MovePosition(objectRb.position + direction * pushPullSpeed * Time.fixedDeltaTime);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //점프패드와 충돌
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
        // IInteractable 인터페이스를 구현한 오브젝트를 찾음
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
        // 상호작용 가능한 오브젝트 범위를 벗어났을 때
        if (other.GetComponent<IInteractable>() != null)
        {
            interactableObj = null;
        }
        if (other.CompareTag("Object")) // 물체를 벗어나면
        {
            isNearObject = false; // 물체 근처 여부 해제
        }
    }

    //로프 매달리기
    private void AttachToRope(Rigidbody2D ropeSegment)
    {
        ropeJoint.enabled = true;
        ropeJoint.connectedBody = ropeSegment;
        isClimbing = true;
    }
    //로프에서 떨어지기
    private void DetachFromRope()
    {
        ropeJoint.enabled = false;
        ropeJoint.connectedBody = null;
        isClimbing = false;
    }
}