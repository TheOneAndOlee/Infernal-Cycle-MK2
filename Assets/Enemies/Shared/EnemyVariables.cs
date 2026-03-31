using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    #region Variable Getters and Setters

    [Tooltip("This enemy's max health")]
    [SerializeField]
    private float _maxHealth;
    private float _health;

    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, _maxHealth);
        }
    }

    [Tooltip("The amount of damage this enemy does to the player")]
    [SerializeField]
    private float _damage;

    public float Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }

    [Tooltip("The movement speed of this enemy")]
    [SerializeField]
    private float _movementSpeed;

    public float movementSpeed
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

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleEnemyHealthUpdates()
    {
        if (_health < 0)
        {
            // Die
        }

        // Do other stuff (if applicable)
    }
}
