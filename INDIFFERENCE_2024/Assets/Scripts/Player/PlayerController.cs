using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;

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
    public float climbSpeed = 3f;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public float input;
    [HideInInspector] public IInteractable interactableObj;
    public float interactableRange = 2f;
    public bool canMove = true;
    [HideInInspector] public bool isClimbing = false;
    private HingeJoint2D ropeJoint;
    private Animator animator;
    private bool isOnJumpPad = false;
    private bool isNearObject = false;
    private bool isInverted = false;

    public bool isOn_Mp = false;
    public Rigidbody2D platformRb;

    public float fallMultiplier = 1.5f;
    private float maxFallSpeed = 19.6f;

    private BoxCollider2D boxCollider;
    private Vector2 originalColliderSize;
    [HideInInspector] public bool isCrouching = false;
    [HideInInspector] public bool isRunning = false;
    public float crouchSpeedMultiplier = 0.5f;

    public GameObject interactionTextPrefab; 
    private GameObject currentInteractionText; 
    public float floatSpeed = 1f; 
    public float floatAmount = 0.2f;

    private PlayerAnimator playerAni;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    public LayerMask wallLayer;
    public Transform wallRightCheck;
    public Transform wallLeftCheck;
    public bool isWallJumping;
    public float wallJumpingDuration;
    public Vector2 wallJumpingPower;

    private GameObject textObject;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ropeJoint = GetComponent<HingeJoint2D>();
        ropeJoint.enabled = false;
        boxCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = boxCollider.size;
        playerAni = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        if (canMove)
        {
            // 플레이어 이동 입력 처리
            input = Input.GetAxisRaw("Horizontal");

            if (isInverted)
            {
                input *= -1;
            }


            if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            else if (isGrounded && Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;
            }
            if (isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = true;
                boxCollider.size = new Vector2(boxCollider.size.x, originalColliderSize.y / 2f);
            }
            else if (isGrounded && Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCrouching = false;
                boxCollider.size = originalColliderSize;
            }
            // 점프
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            // 상호작용
            if (Input.GetKeyDown(KeyCode.F) && interactableObj != null)
            {
                interactableObj.Interact();
            }
            if (isNearObject && Input.GetKey(KeyCode.C)) 
            {
                PushOrPullObject();
            }
            else
            {
                rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);
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
            WallSlide();

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

        if (currentInteractionText != null)
        {
            Vector3 originalPosition = interactableObj != null
                ? (interactableObj as MonoBehaviour).transform.position : transform.position;
            currentInteractionText.transform.position = originalPosition + new Vector3(0, 1 + Mathf.Sin(Time.time * floatSpeed) * floatAmount, 0);
        }
    }

    public void InvertControls(bool invert)
    {
        isInverted = invert;
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
        if(isWallJumping)
        {
            rb.velocity = new Vector2(- input * wallJumpingPower.x, wallJumpingPower.y);
        }
    }
    void Jump()
    {
        if(isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if(isWallSliding)
        {
            isWallJumping = true;
            Invoke("StopWallJump", wallJumpingDuration);
        }
    }
    void StopWallJump()
    {
        isWallJumping = false;
    }
    private bool IsRightWalled()
    {
        return Physics2D.OverlapCircle(wallLeftCheck.position, 0.1f, wallLayer);
    }
    private bool IsLeftWalled()
    {
        return Physics2D.OverlapCircle(wallRightCheck.position, 0.1f, wallLayer);
    }
    private void WallSlide()
    {
        if(IsRightWalled() && !isGrounded && input != 0f || IsLeftWalled() && !isGrounded && input != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    
    void PushOrPullObject()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object"))
            {
                Vector2 direction = new Vector2(input, 0).normalized;
                Rigidbody2D objectRb = hitCollider.GetComponent<Rigidbody2D>();

                if (objectRb != null && objectRb.bodyType == RigidbodyType2D.Dynamic)
                {
                    objectRb.velocity = direction * pushPullSpeed;
                }
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
        var newInteractableObj = other.GetComponent<IInteractable>();

        if (newInteractableObj != null)
        {
            interactableObj = newInteractableObj;
            if (other.CompareTag("Interactable"))
            {
                ShowInteractionText(other.transform, "[F] 상호작용");
            }
        }
        else if (other.CompareTag("Object")) 
        {
            isNearObject = true;
            //ShowInteractionText(other.transform, "C 밀고/당기기");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IInteractable>() == interactableObj)
        {
            interactableObj = null;
            HideInteractionText();
        }

        if (other.CompareTag("Object"))
        {
            isNearObject = false;
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
    private void ShowInteractionText(Transform target, string text)
    {
        if (interactionTextPrefab != null && currentInteractionText == null)
        {
            Vector3 spawnPosition = new Vector3(target.position.x, target.position.y + 10f, 0f); 
            currentInteractionText = Instantiate(interactionTextPrefab, spawnPosition, Quaternion.identity);
            TextMeshPro tmp = currentInteractionText.GetComponent<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = text; 
            }
            Renderer renderer = currentInteractionText.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = "UI";
                renderer.sortingOrder = 10; 
            }
        }
    }

    // 상호작용 텍스트 숨기기
    private void HideInteractionText()
    {
        if (currentInteractionText != null)
        {
            Destroy(currentInteractionText); 
            currentInteractionText = null;
        }
    }
}