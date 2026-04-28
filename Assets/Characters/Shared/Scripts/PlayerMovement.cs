using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    #region Variable Getters & Setters
    //[Tooltip("Note: Do not change this variable, change DefaultMovementSpeed. Horizontal Movement Speed")]
    //[SerializeField]
    private float _moveSpeed = 10f;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    [Tooltip("Default Horizontal Movement Speed")]
    [SerializeField]
    private float _defaultMoveSpeed = 10f;

    public float DefaultMoveSpeed
    {
        get => _defaultMoveSpeed;
        set => _defaultMoveSpeed = value;
    }

    //[Tooltip("Note: Do not change this value, change DefaultJumpForce. Vertical Jump Force")]
    //[SerializeField]
    private float _jumpForce = 10f;

    public float JumpForce
    {
        get => _jumpForce;
        set => _jumpForce = value;
    }

    [Tooltip("Default Jump Force")]
    [SerializeField]
    private float _defaultJumpForce = 10f;

    public float DefaultJumpForce
    {
        get => _defaultJumpForce;
        set => _defaultJumpForce = value;
    }

    [Tooltip("Multiplier applied to gravity when space bar is released early")]
    [SerializeField]
    private float _shortHopGravityMultiplier = 2.5f;

    public float ShortHopGravityMultiplier
    {
        get => _shortHopGravityMultiplier;
        set => _shortHopGravityMultiplier = value;
    }

    //[Tooltip("Dash Duration in seconds")]
    //[SerializeField]
    private float _dashDuration = 0.2f;

    public float DashDuration
    {
        get => _dashDuration;
        set => _dashDuration = value;
    }

    [Tooltip("Default Dash Duration in Seconds")]
    [SerializeField] 
    private float _defaultDashDuration = 0.2f;

    public float DefaultDashDuration
    {
        get => _defaultDashDuration;
        set => _defaultDashDuration = value;
    }

    //[Tooltip("Dash Cooldown in seconds")]
    //[SerializeField]
    private float _dashCooldown = 1f;

    public float DashCooldown
    {
        get => _dashCooldown;
        set => _dashCooldown = value;
    }

    [Tooltip("Default Dash Cooldown in Seconds")]
    [SerializeField]
    private float _defaultDashCooldown = 1f;

    public float DefaultDashCooldown
    {
        get => _defaultDashCooldown;
        set => _defaultDashCooldown = value;
    }

    //[Tooltip("Player speed while dashing")]
    //[SerializeField]
    private float _dashSpeed = 15f;

    public float DashSpeed
    {
        get => _dashSpeed;
        set => _dashSpeed = value;
    }

    [Tooltip("Default Dash Speed")]
    [SerializeField]
    private float _defaultDashSpeed = 15f;

    public float DefaultDashSpeed
    {
        get => _defaultDashSpeed;
        set => _defaultDashSpeed = value;
    }

    private float _lastHorizontalDirection = 1f;
    public float LastHorizontalDirection
    {
        get => _lastHorizontalDirection;
        set => _lastHorizontalDirection = value;
    }


    private bool isDashing = false;
    private bool canDash = false;

    private float jumpBufferCounter;

    private float defaultGravity;
    private bool isJumpButtonHeld;
    private float horizontalInput;

    private Rigidbody2D rbody;
    private Transform playerTransform;

    private Vector2 movementVector;

    private PlayerCollision playerCollision;

    // Animations
    [SerializeField]
    private Animator _animator;

    #endregion

    private void Start()
    {
        playerTransform = transform;
        rbody = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<PlayerCollision>();
        defaultGravity = rbody.gravityScale;
        canDash = true;

        MoveSpeed = DefaultMoveSpeed;
        JumpForce = DefaultJumpForce;
        DashSpeed = DefaultDashSpeed;
        DashDuration = DefaultDashDuration;
        DashCooldown = DefaultDashCooldown;
    }

    #region Update & FixedUpdate
    private void Update()
    {
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f)
        {
            if (playerCollision.IsOnPlatform() && movementVector.y < -0.5f)
            {
                LedgeDrop();
                jumpBufferCounter = 0;
            }
            else if (playerCollision.IsOnPlatform() || playerCollision.IsGrounded())
            {
                rbody.linearVelocity = new Vector2(rbody.linearVelocityX, _jumpForce);
                jumpBufferCounter = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rbody.linearVelocity = new Vector2(horizontalInput * _moveSpeed, rbody.linearVelocityY);

        if (rbody.linearVelocityY > 0 && !isJumpButtonHeld)
        {
            rbody.gravityScale = defaultGravity * _shortHopGravityMultiplier;
        }
        else
        {
            rbody.gravityScale = defaultGravity;
        }
    }

    private void UpdateAnimations()
    {
        //Debug.Log(_animator.GetBool("IsMoving") + " " + _animator.GetBool("IsJumping") + " " + _animator.GetBool("IsFalling"));

        if (_animator != null)
        {
            bool isGrounded = playerCollision.IsGrounded() || playerCollision.IsOnPlatform();
            _animator.SetBool("IsMoving", isGrounded && Mathf.Abs(horizontalInput) > 0.01f);
            _animator.SetBool("IsJumping", !isGrounded && rbody.linearVelocityY > 0.1f);
            _animator.SetBool("IsFalling", !isGrounded && rbody.linearVelocityY < -0.1f);
        }
    }

    private void LateUpdate()
    {
        if (_lastHorizontalDirection != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -_lastHorizontalDirection;
            transform.localScale = scale;
        }

        UpdateAnimations();
    }

    #endregion

    #region Movement Methods    

    private void LedgeDrop()
    {
        if (playerCollision.IsOnPlatform())
        {
            StartCoroutine(DropThroughPlatform());
        }
    }

    private IEnumerator DropThroughPlatform()
    {
        Debug.Log("Dropping through platform...");

        playerCollision.DisableCollision("Platform");

        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => !playerCollision.IsOnPlatform());

        playerCollision.EnableCollision("Platform");
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rbody.gravityScale = 0;

        canDash = false;

        float dashDirection = Mathf.Abs(horizontalInput) > 0.01f ? Mathf.Sign(horizontalInput) : _lastHorizontalDirection;

        rbody.linearVelocity = new Vector2(dashDirection * _dashSpeed, 0);

        playerCollision.DisableCollision("Enemy");

        yield return new WaitForSeconds(_dashDuration);

        playerCollision.EnableCollision("Enemy");

        rbody.gravityScale = defaultGravity;
        isDashing = false;
    }

    private IEnumerator GoOnDashCooldown()
    {
        yield return new WaitForSeconds(_dashCooldown);
        canDash = true;
    }

    #endregion

    #region On Methods (OnMove, OnCollisionEnter, etc.)

    public void OnMove(InputValue value)
    {
        // Implement movement logic here
        horizontalInput = value.Get<Vector2>().x;

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            _lastHorizontalDirection = Mathf.Sign(horizontalInput);
        }

        movementVector.y = value.Get<Vector2>().y;
    }

    public void OnDash(InputValue value)
    {
        if (!value.isPressed || isDashing || !canDash)
        {
            return;
        }

        Debug.Log("Dash called");
        StartCoroutine(Dash());
        StartCoroutine(GoOnDashCooldown());
    }
    public void OnJump(InputValue value)
    {

        isJumpButtonHeld = value.isPressed;

        if (isJumpButtonHeld)
        {
            jumpBufferCounter = 0.2f;
        }
    }

    public void OnTestButton(InputValue value)
    {
        if (!value.isPressed || ShopBehaviour.Instance == null)
        {
            return;
        }

        if (ShopBehaviour.Instance.isActive)
        {
            ShopBehaviour.Instance.DisableShopMenu();
        }
        else
        {
            ShopBehaviour.Instance.OpenItemShop();
        }
    }

    #endregion

}