using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    #region Variable Getters & Setters
    [Tooltip("Horizontal Movement Speed")]
    [SerializeField]
    private float _moveSpeed = 10f;

    public float MoveSpeed
    {
        get
        {
            return _moveSpeed;
        }
        set
        {
            _moveSpeed = value;
        }
    }

    [Tooltip("Vertical Jump Force")]
    [SerializeField]
    private float _jumpForce = 10f;

    public float JumpForce
    {
        get
        {
            return _jumpForce;
        }
        set
        {
            _jumpForce = value;
        }
    }

    [Tooltip("Multiplier applied to gravity when space bar is released early")]
    [SerializeField]
    private float _shortHopGravityMultiplier = 2.5f;

    public float ShortHopGravityMultiplier
    {
        get
        {
            return _shortHopGravityMultiplier;
        }
        set
        {
            _shortHopGravityMultiplier = value;
        }
    }

    //[Tooltip("Buffer time for a pre-inputted jump")]
    //[SerializeField]
    //private float _jumpBufferTime = 0.2f;

    [Tooltip("Dash Duration in seconds")]
    [SerializeField]
    private float _dashDuration = 0.2f;

    [Tooltip("Dash Cooldown in seconds")]
    [SerializeField]
    private float _dashCooldown = 1f;

    public float DashDuration
    {
        get
        {
            return _dashDuration;
        }
        set
        {
            _dashDuration = value;
        }
    }

    [Tooltip("Player speed while dashing")]
    [SerializeField]
    private float _dashSpeed = 15f;

    private float DashSpeed
    {
        get
        {
            return _dashSpeed;
        }

        set
        {
            _dashSpeed = value;
        }
    }

    private bool isDashing = false;
    private bool canDash = false;

    private float jumpBufferCounter;

    private float defaultGravity;
    private bool isJumpButtonHeld;
    private float horizontalInput;
    private float lastHorizontalDirection = 1f;

    private Rigidbody2D rbody;
    private Transform playerTransform;

    private Vector2 movementVector;

    private LayerMask groundLayer;
    private LayerMask platformLayer;

    #endregion

    private void Start()
    {
        playerTransform = transform;
        rbody = GetComponent<Rigidbody2D>();
        defaultGravity = rbody.gravityScale;
        canDash = true;

        groundLayer = LayerMask.GetMask("Ground");
        platformLayer = LayerMask.GetMask("Platform");
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
            if (IsOnPlatform() && movementVector.y < -0.5f)
            {
                ledgeDrop();
                jumpBufferCounter = 0;
            }
            else if (IsOnPlatform() || IsGrounded())
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

    #endregion

    #region Movement Methods    

    private void ledgeDrop()
    {
        if (IsOnPlatform())
        {
            StartCoroutine(DropThroughPlatform());
        }
    }

    private IEnumerator DropThroughPlatform()
    {
        Debug.Log("Dropping through platform...");

        disableCollision("Platform");

        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => !IsOnPlatform());

        enableCollision("Platform");
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rbody.gravityScale = 0;

        canDash = false;

        float dashDirection = Mathf.Abs(horizontalInput) > 0.01f ? Mathf.Sign(horizontalInput) : lastHorizontalDirection;

        rbody.linearVelocity = new Vector2(dashDirection * _dashSpeed, 0);

        disableCollision("Enemy");

        yield return new WaitForSeconds(_dashDuration);

        enableCollision("Enemy");

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

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            lastHorizontalDirection = Mathf.Sign(horizontalInput);
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

        if (value.isPressed)
        {
            jumpBufferCounter = 0.2f;

            //Debug.Log("Is On Platform Status: " + IsOnPlatform());
            //Debug.Log("Downwards Input: " + movementVector.y);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
    }

    #endregion

    #region Helper Functions
    public bool IsOnPlatform()
    {
        float raycastDist = 1.2f;
        RaycastHit2D raycast = Physics2D.Raycast(playerTransform.position, Vector2.down, raycastDist, platformLayer);

        return raycast.collider != null;
    }

    public bool IsGrounded()
    {
        float raycastDist = 1.2f;
        RaycastHit2D raycast = Physics2D.Raycast(playerTransform.position, Vector2.down, raycastDist, groundLayer);

        return raycast.collider != null;
    }

    public void disableCollision(string LayerName)
    {
        int platformLayerNum = LayerMask.NameToLayer(LayerName);

        Debug.Log(platformLayerNum);

        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayerNum, true);
    }

    public void enableCollision(string LayerName)
    {
        int platformLayerNum = LayerMask.NameToLayer(LayerName);

        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayerNum, false);
    }

    #endregion

}
