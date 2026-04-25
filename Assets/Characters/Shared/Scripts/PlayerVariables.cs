using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerVariables : MonoBehaviour
{
    #region Variables and their Getters & Setters

    [Tooltip("Player Health")]
    [SerializeField] 
    private float _maxHealth = 100f;
    private float _health;

    private float originalMaxHealth;

    public float MaxHealth
    {
        get => _maxHealth;
       
        set
        {
            originalMaxHealth = _maxHealth;
            _maxHealth = value;
            UpdateMaxHealth();
        }
    }

    public float Health
    {
        get => _health;

        set
        {
            // Healing, so don't worry about i-frames
            if (value > _health)
            {
                _health += value;
                _health = Math.Clamp(value, 0, _maxHealth);
                HandleHealthUpdates();
            }

            if (!Damageable)
            {
                Debug.Log("Player is currently invincible!");
                return;
            }

            // We did take damage, so add i-frames
            Damageable = false;
            _health += value;
            _health = Math.Clamp(value, 0, _maxHealth);
            HandleHealthUpdates();
            StartCoroutine(IFrames());
            
        }
    }

    [Tooltip("Damage Multiplier for ALL player attacks")]
    [SerializeField] private float _damageMultiplier = 1f;

    public float DamageMultiplier
    {
        get => _damageMultiplier;
        set => _damageMultiplier = value;
    }

    [Tooltip("Duration of i-frames in seconds")]
    [SerializeField]
    private float iFrameDuration = 1f;

    public float IFrameDuration
    {
        get => iFrameDuration;
        set => iFrameDuration = value;
    }

    // Money for upgrades & items
    private float _money = 0f;
    public float Money
    {
        get => _money;
        set => _money = value;
    }

    private PlayerInput _playerInput;
    private bool Damageable = true;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.ActivateInput();
    }

    #region Methods

    public void OnTestButton2(InputValue value)
    {
        Debug.Log("Adding 100 money to player");
        Money += 100;
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

    private IEnumerator IFrames()
    {
        Debug.Log("Player is now invulnerable for " + iFrameDuration + " seconds.");
        yield return new WaitForSeconds(iFrameDuration);
        Damageable = true;
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
