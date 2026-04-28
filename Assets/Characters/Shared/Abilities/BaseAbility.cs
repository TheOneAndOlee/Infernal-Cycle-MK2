using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "BaseAbility", menuName = "Scriptable Objects/BaseAbility")]
public abstract class BaseAbility : ScriptableObject
{
    #region Properties

    [Header("Ability Properties")]

    [Tooltip("The name of the ability")]
    [SerializeField]
    private string _abilityName;

    public string AbilityName
    {
        get => _abilityName;
    }

    [Tooltip("The cooldown for the ability in seconds")]
    [SerializeField]
    private float _abilityCooldown;

    public float AbilityCooldown
    {
        get => _abilityCooldown;
        set => _abilityCooldown = value;
    }

    [Tooltip("The description for the ability")]
    [SerializeField]
    private string _abilityDescription;

    [Tooltip("The duration of the ability or it's effects, if applicable")]
    [SerializeField]
    private float _abilityDuration;

    public float AbilityDuration
    {
        get => _abilityDuration;
        set => _abilityDuration = value;
    }

    protected bool isOnCooldown = false;
    #endregion

    #region Methods

    private void OnEnable()
    {
        isOnCooldown = false;
    }

    public virtual void UseAbility(AbilityManager manager)
    {
        if (isOnCooldown)
        {
            return;
        }

        manager.StartCoroutine(GoOnCooldown());
    }

    public virtual IEnumerator PassiveAbilityEffect(AbilityManager manager, int stacks)
    {
        yield break;
    }

    #endregion 

    private IEnumerator GoOnCooldown()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(_abilityCooldown);

        isOnCooldown = false;
    }
}
