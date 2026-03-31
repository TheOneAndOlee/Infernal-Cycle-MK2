using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerVariables : MonoBehaviour
{
    #region Variables and their Getters & Setters

    [Tooltip("Player Health")]
    [SerializeField] 
    private float _maxHealth = 100f;
    private float _health;

    private float originalMaxHealth;

    private float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            originalMaxHealth = _maxHealth;
            _maxHealth = value;
            UpdateMaxHealth();
        }
    }

    private float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Math.Clamp(value, 0, _maxHealth);
            HandleHealthUpdates();
        }
    }

    [Tooltip("Damage Multiplier for ALL player attacks")]
    [SerializeField] private float _damageMultiplier = 1f;

    public float DamageMultiplier
    {
        get
        {
            return _damageMultiplier;
        }
        set
        {
            _damageMultiplier = value;
        }
    }

    private PlayerInput _playerInput;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.ActivateInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Methods

    public void OnTestButton(InputValue value)
    {
        Debug.Log("Subtracting 25 health from player");
        Health -= 25;
    }

    private void HandleHealthUpdates()
    {
        Debug.Log("Player Health: " + _health + "/" + _maxHealth);
        if (_health <= 0)
        {
            Die();
        }

        // Update Health Bar
    }

    private void UpdateMaxHealth()
    {
        if (_maxHealth != originalMaxHealth)
        {
            float healthRatio = _maxHealth / originalMaxHealth;

            _health *= healthRatio;
        }

        // Update UI if needed
    }

    private void Die()
    {
        Debug.LogWarning("The player is dead. Not big surprise.");
        _playerInput.DeactivateInput();
    }

    #endregion
}
