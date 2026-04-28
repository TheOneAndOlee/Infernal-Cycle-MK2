using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

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
                _health += (value * HealingMultiplier);
                _health = Math.Clamp(value, 0, MaxHealth);
                HandleHealthUpdates();
                return;
            }

            if (!Damageable)
            {
                Debug.Log("Player is currently invincible!");
                return;
            }

            // We did take damage, so add i-frames
            Damageable = false;
            _health += (value * IncomingDamageMultiplier);
            _health = Math.Clamp(value, 0, MaxHealth);
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

    [Tooltip("Incoming Damage Multiplayer for the player")]
    [SerializeField] private float _incomingDamageMultiplier = 1f;

    public float IncomingDamageMultiplier
    {
        get => _incomingDamageMultiplier;
        set => _incomingDamageMultiplier = value;
    }

    [Tooltip("Healing Multiplier for the player")]
    [SerializeField] private float _healingMultiplier = 1f;

    public float HealingMultiplier
    {
        get => _healingMultiplier;
        set => _healingMultiplier = value;
    }


    [Tooltip("Duration of i-frames in seconds")]
    [SerializeField]
    private float iFrameDuration = 0.2f;

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
        set
        {
            _money = value;
            OnMoneyChanged.Invoke();
        }
    }
    public UnityEvent OnMoneyChanged;

    private PlayerInput _playerInput;
    private bool Damageable = true;

    [SerializeField] private GameObject playerUI;

    private Image HealthBar;

    #endregion

    public static PlayerVariables Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.ActivateInput();

        var imageComponents = playerUI.GetComponentsInChildren<Image>();

        foreach (var component in imageComponents )
        {
            if (component.name == "HealthBar")
            {
                HealthBar = component;
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
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
        
        var ratio = _health / _maxHealth;

        HealthBar.fillAmount = ratio;
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

        HandleHealthUpdates();
    }

    private void Die()
    {
        Debug.LogWarning("The player is dead. Not big surprise.");
        _playerInput.DeactivateInput();
    }

    #endregion
}
