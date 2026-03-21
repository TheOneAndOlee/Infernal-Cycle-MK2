using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Horizontal Movement Speed")]
    [SerializeField]
    private float moveSpeed = 10f;

    [Tooltip("Vertical Jump Force")]
    [SerializeField]
    private float jumpForce = 10f;

    [Tooltip("Multiplier applied to gravity when space bar is released early")]
    [SerializeField]
    private float shortHopGravityMultiplier = 2.5f;

    [Tooltip("Buffer time for a pre-inputted jump")]
    [SerializeField]
    private float jumpBufferTime = 0.2f;

    private float jumpBufferCounter;

    private float defaultGravity;
    private bool isJumpButtonHeld;
    private float horizontalInput;

    private Rigidbody2D rbody;
    private Transform playerTransform;

    private Vector2 movementVector;

    private LayerMask groundLayer;
    private LayerMask platformLayer;

    private void Start()
    {
        playerTransform = transform;
        rbody = GetComponent<Rigidbody2D>();
        defaultGravity = rbody.gravityScale;

        groundLayer = LayerMask.GetMask("Ground");
        platformLayer = LayerMask.GetMask("Platform");
    }

    #region Methods    
    private void FixedUpdate()
    {
        rbody.linearVelocity = new Vector2(horizontalInput * moveSpeed, rbody.linearVelocityY);
    
        if (rbody.linearVelocityY > 0 && !isJumpButtonHeld)
        {
            rbody.gravityScale = defaultGravity * shortHopGravityMultiplier;
        }
        else
        {
            rbody.gravityScale = defaultGravity;
        }
    }

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
                rbody.linearVelocity = new Vector2(rbody.linearVelocityX, jumpForce);
                jumpBufferCounter = 0f;
            }
        }
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

        int platformLayerNum = LayerMask.NameToLayer("Platform");

        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayerNum, true);

        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => !IsOnPlatform());

        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayerNum, false);
    }

    public void OnMove(InputValue value)
    {
        // Implement movement logic here
        horizontalInput = value.Get<Vector2>().x;

        movementVector.y = value.Get<Vector2>().y;
    }

    private void dash()
    {
        // Implement dash logic here
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
    }

    public bool IsGrounded()
    {
        float raycastDist = 1.2f;
        RaycastHit2D raycast = Physics2D.Raycast(playerTransform.position, Vector2.down, raycastDist, groundLayer);
        
        return raycast.collider != null;
    }

    public bool IsOnPlatform()
    {
        float raycastDist = 1.2f;
        RaycastHit2D raycast = Physics2D.Raycast(playerTransform.position, Vector2.down, raycastDist, platformLayer);

        return raycast.collider != null;
    }

    #endregion

}
