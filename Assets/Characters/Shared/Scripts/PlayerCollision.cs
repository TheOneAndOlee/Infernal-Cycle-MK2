using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerVariables playerVariables;
    private LayerMask enemyLayer;
    private LayerMask groundLayer;
    private LayerMask platformLayer;
    private void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        platformLayer = LayerMask.GetMask("Platform");
        enemyLayer = LayerMask.GetMask("Enemy");
        playerTransform = transform;
        playerVariables = GetComponent<PlayerVariables>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger Collision Detected!");

        if (collision == null)
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox"))
        {
            //Debug.Log("Collided with enemy: " + collision.gameObject.name);
            
            float incomingDamage = collision.gameObject.GetComponentInParent<EnemyVariables>().Damage;
            playerVariables.Health -= incomingDamage;
        }
        //else
        //{
        //    Debug.Log("Collided with: " + collision.gameObject.name);
        //}
    }

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

    public void DisableCollision(string LayerName)
    {
        int platformLayerNum = LayerMask.NameToLayer(LayerName);

        //Debug.Log(platformLayerNum);

        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayerNum, true);
    }

    public void EnableCollision(string LayerName)
    {
        int platformLayerNum = LayerMask.NameToLayer(LayerName);

        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayerNum, false);
    }

    #endregion
}
