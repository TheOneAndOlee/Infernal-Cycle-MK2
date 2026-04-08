using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Getters and Setters

    [Tooltip("The movement speed of this enemy")]
    [SerializeField]
    private float _movementSpeed;

    public float MovementSpeed
    {
        get
        {
            return _movementSpeed;
        }
        set
        {
            _movementSpeed = value;
        }
    }

    [Tooltip("The minimum amount of time spent wandering")]
    [SerializeField]
    private float _minWanderTime;

    public float MinWanderTime
    {
        get => _minWanderTime;
        set => _minWanderTime = value;
        
    }

    [Tooltip("The maximum amount of time spent wandering")]
    [SerializeField]
    private float _maxWanderTime;

    public float MaxWanderTime
    {
        get
        {
            return _maxWanderTime;
        }
        set
        {
            _maxWanderTime = value;
        }
    }

    private Rigidbody2D rbody;

    #endregion

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    //private void FixedUpdate()
    //{
    //    rbody.gravityScale = 0f;
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            // Pick a random X direction (-1 for left, 1 for right)
            float direction = Random.Range(0, 2) == 0 ? -1f : 1f;

            // Pick a random amount of time within the specified range
            float wanderTime = Random.Range(_minWanderTime, _maxWanderTime);
            float elapsedTime = 0f;

            // Move for the random amount of time
            while (elapsedTime < wanderTime)
            {
                // Move enemy in the chosen direction
                rbody.linearVelocity = new Vector2(direction * _movementSpeed, rbody.linearVelocityY);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Stop moving after the wander time is over
            rbody.linearVelocity = new Vector2(0f, rbody.linearVelocityY);

            // A short pause before wandering again
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
}
