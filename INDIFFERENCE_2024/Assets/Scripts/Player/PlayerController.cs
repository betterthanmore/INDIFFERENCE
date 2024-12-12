using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeedMultiplier = 1.5f;
    public float pushPullSpeed = 3f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public float rollSpeed = 20f;
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
    private bool isPushorPulling = false;
    public bool isOn_Mp = false;
    public Rigidbody2D platformRb;

    public float fallMultiplier = 1.5f;
    private float maxFallSpeed = 19.6f;

    [HideInInspector] public bool isRunning = false;

    public GameObject interactionTextPrefab; 
    private GameObject currentInteractionText; 
    public float floatSpeed = 1f; 
    public float floatAmount = 0.2f;

    private Animator playerAni;

    public bool isTransparency = false;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    public LayerMask wallLayer;
    public Transform wallCheck;
    public bool isWallJumping;
    public float wallJumpingDuration;
    public Vector2 wallJumpingPower;
    private bool isRolling = false;

    public int extraJumpCount = 1;
    private int currentJumpCount = 0;
    public bool activatedDJ = false; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ropeJoint = GetComponent<HingeJoint2D>();
        ropeJoint.enabled = false;
        playerAni = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isRolling)
            return;
        if (canMove)
        {
            // �÷��̾� �̵� �Է� ó��
            input = Input.GetAxisRaw("Horizontal");
            if(input != 0 && isGrounded)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
            if (isInverted)
            {
                input *= -1;
            }
            if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
                animator.SetBool("isRunning", isRunning);
            }
            else if (isGrounded && Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;
                animator.SetBool("isRunning", isRunning);
            }
            else if (!isGrounded && Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;
                animator.SetBool("isRunning", isRunning);
            }
            // ����
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            // ��ȣ�ۿ�
            if (Input.GetKeyDown(KeyCode.F ) && interactableObj != null && !isNearObject)
            {
                interactableObj.Interact();
                animator.SetTrigger("Interaction");
            }
            if (Input.GetKey(KeyCode.F) && isNearObject)
            {
                 PushOrPullObject();
            }
            else if(Input.GetKeyUp(KeyCode.F))
            {
                isPushorPulling = false;
                animator.SetBool("isPulling", false);
                animator.SetBool("isPushing", false);
            }
            else
            {
                rb.velocity = new Vector2(input * moveSpeed, rb.velocity.y);
            }
            if (Input.GetKey(KeyCode.F) && !isClimbing)
            {
                // ������ �������� Ȯ��
                Collider2D rope = Physics2D.OverlapCircle(transform.position, 0.2f, ropeLayer);
                if (rope != null)
                {
                    AttachToRope(rope.attachedRigidbody);
                }
            }
            WallSlide();
            Flip();
            // �������� ��������
            if (Input.GetKeyUp(KeyCode.F) && isClimbing)
            {
                DetachFromRope();
            }
            HandleMovementSound();
        }

        // �÷��̾ ���� �ִ��� üũ
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
        else if(isGrounded && rb.velocity.y <0)
        {
            animator.SetTrigger("Fall");
        }
        if (rb.velocity.y != 0 || !isGrounded)
        {
            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }

        if (currentInteractionText != null)
        {
            Vector3 originalPosition = interactableObj != null
                ? (interactableObj as MonoBehaviour).transform.position : transform.position;
            currentInteractionText.transform.position = originalPosition + new Vector3(0, 1 + Mathf.Sin(Time.time * floatSpeed) * floatAmount, 0);
        }
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void InvertControls(bool invert)
    {
        isInverted = invert;
    }
    public void StartRoll()
    {
        if(!isRolling)
        {
            animator.SetTrigger("Roll");
            isRolling = true;
            float direction = input != 0 ? input : Mathf.Sign(transform.localScale.x); rb.velocity = Vector2.zero;
            Vector2 rollDirection = new Vector2(direction, 0).normalized;
            rb.AddForce(rollDirection * rollSpeed, ForceMode2D.Impulse); 
            Invoke("EndRoll", 0.5f);
        }
    }
    public void EndRoll()
    {
        isRolling = false;
        rb.velocity = Vector2.zero;
    }
    private void Flip()
    {
        if(!isPushorPulling)
        {
            if (input > 0.01) this.transform.localScale = new Vector3(1, 1, 1);
            if (input < -0.01) this.transform.localScale = new Vector3(-1, 1, 1);
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
        else
        {
            currentSpeed = moveSpeed;
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
        animator.SetTrigger("Jump");
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_JUMP);
            currentJumpCount = 0; 
        }
        else if (currentJumpCount < extraJumpCount && activatedDJ)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_JUMP);
            currentJumpCount++;
        }

        if (isWallSliding)
        {
            isWallJumping = true;
            Invoke("StopWallJump", wallJumpingDuration);
        }
    }
    void StopWallJump()
    {
        isWallJumping = false;
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        if(IsWalled() && !isGrounded && input != 0f)
        {
            isWallSliding = true;
            animator.SetBool("isWallSliding", true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("isWallSliding", false);
        }
    }
    void PushOrPullObject()
    {
        if (!isNearObject) return;
        isPushorPulling = true;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(wallCheck.position, 0.2f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object") && isPushorPulling)
            {
                Vector2 direction = new Vector2(input, 0).normalized;
                Rigidbody2D objectRb = hitCollider.GetComponent<Rigidbody2D>();

                if (objectRb != null && objectRb.bodyType == RigidbodyType2D.Dynamic)
                {
                    objectRb.velocity = direction * pushPullSpeed;

                    if (transform.localScale.x < 0 && input > 0)
                    {
                        animator.SetBool("isPushing", false);
                        animator.SetBool("isPulling", true);
                        animator.speed = Mathf.Abs(input);
                    }
                    else if (transform.localScale.x > 0 && input < 0)
                    {
                        animator.SetBool("isPushing", false);
                        animator.SetBool("isPulling", true);
                        animator.speed = Mathf.Abs(input);
                    }
                    else if (transform.localScale.x < 0 && input < 0)
                    {
                        animator.SetBool("isPushing", true);
                        animator.SetBool("isPulling", false);
                        animator.speed = Mathf.Abs(input);
                    }
                    else if (transform.localScale.x > 0 && input > 0)
                    {
                        animator.SetBool("isPushing", true);
                        animator.SetBool("isPulling", false);
                        animator.speed = Mathf.Abs(input);
                    }
                    else
                    {
                        animator.speed = 1f;
                    }
                }
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
        var newInteractableObj = other.GetComponent<IInteractable>();

        if (newInteractableObj != null)
        {
            interactableObj = newInteractableObj;
            if (other.CompareTag("Interactable"))
            {
                ShowInteractionText(other.transform, "[F]");
            }
        }
        if (other.CompareTag("Object"))
        {
            isNearObject = true;
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

    //���� �Ŵ޸���
    private void AttachToRope(Rigidbody2D ropeSegment)
    {
        animator.SetBool("Hang", true);
        animator.SetBool("Jumping", false);
        rb.constraints = RigidbodyConstraints2D.None;
        ropeJoint.enabled = true;
        ropeJoint.connectedBody = ropeSegment;
        isClimbing = true;
    }
    //�������� ��������
    private void DetachFromRope()
    {
        animator.SetBool("Hang", false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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

    // ��ȣ�ۿ� �ؽ�Ʈ �����
    private void HideInteractionText()
    {
        if (currentInteractionText != null)
        {
            Destroy(currentInteractionText); 
            currentInteractionText = null;
        }
    }

    private void HandleMovementSound()
    {
        if (isGrounded && input != 0) // ���� �ְ� �����̴� ��
        {
            if (isRunning) // �ٴ� ��
            {
                SoundManager.instance.PlayWalkRunSFX(SoundManager.ESfx.SFX_RUN);
            }
            else // �ȴ� ��
            {
                SoundManager.instance.PlayWalkRunSFX(SoundManager.ESfx.SFX_WALK);
            }
        }
        else
        {
            SoundManager.instance.StopWalkRunSFX(); // ������ ���� �� �ȱ�/�ٱ� �Ҹ� ����
        }
    }
}