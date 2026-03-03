using UnityEngine;

public class PlayerMovement
{
    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float jumpHeight = 2f;


    #region Methods

    private void jump()
    {
        // Implement jump logic here
    }

    private void ledgeDrop()
    {
        // Implement ledge drop logic here
    }

    private void move()
    {
        // Implement movement logic here
    }

    private void dash()
    {
        // Implement dash logic here
    }

    #endregion
}
