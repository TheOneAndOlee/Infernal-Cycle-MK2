using System.Collections;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    #region Variable Getters and Setters

    [Tooltip("This enemy's max health")]
    [SerializeField]
    private float _maxHealth = 100;

    public float MaxHealth
    {
        get => _maxHealth;

        set
        {
            _maxHealth = value;
        }
    }

    private float _health;

    public float Health
    {
        get => _health;
        set
        {
            //_health += value;
            _health = Mathf.Clamp(value, 0, _maxHealth);
            Debug.Log("Enemy health updated: " + _health + "/" + MaxHealth);
            
            HandleEnemyHealthUpdates();            
        }
    }

    [Tooltip("The amount of damage this enemy does to the player")]
    [SerializeField]
    private float _damage;

    public float Damage
    {
        get => _damage;
        set => _damage = value;
        
    }

    [Tooltip("Cooldown for the enemy to repeatedly damage the player in seconds")]
    [SerializeField]
    private float damageCooldown = 1f;

    public float DamageCooldown
    {
        get => damageCooldown;
        set => damageCooldown = value;
    }

    private bool canDamagePlayer = true;

    [Tooltip("How much money the player gets from defeating the enemy")]
    [SerializeField]
    private float moneyOnDeath = 30f;

    #endregion

    private void Awake()
    {
        _health = _maxHealth;
    }

    private void OnColliderEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (canDamagePlayer)
            {
                //Debug.Log("Collided with player. Dealing damage...");
                collision.gameObject.GetComponent<PlayerVariables>().Health -= Damage;
                StartCoroutine(GoOnDamageCooldown());
            }
        }
    }

    private IEnumerator GoOnDamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canDamagePlayer = true;
    }

    private void HandleEnemyHealthUpdates()
    {
        if (_health <= 0)
        {
            Debug.Log("Enemy is dead. Despawning...");

            PlayerVariables.Instance.Money += moneyOnDeath;

            Destroy(gameObject);
        }

        // Do other stuff (if applicable)
    }
}
