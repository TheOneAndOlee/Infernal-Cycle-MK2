using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[CreateAssetMenu(fileName = "BaseAbility", menuName = "Scriptable Objects/BaseAbility")]
public class BaseAbility : ScriptableObject
{
    #region Properties

    [Header("Ability Properties")]

    [Tooltip("The name of the ability")]
    [SerializeField]
    private string _abilityName;

    [Tooltip("The cooldown for the ability in seconds")]
    [SerializeField]
    private float _abilityCooldown;

    [Tooltip("The description for the ability")]
    [SerializeField]
    private string _abilityDescription;

    [Tooltip("The duration of the ability or it's effects, if applicable")]
    [SerializeField]
    private float _abilityDuration;

    private bool isOnCooldown = false;
    #endregion

    #region Getters and Setters

    private string AbilityName
    {
        get
        {
            return _abilityName;
        }
    }

    private float AbilityCooldown
    {
        get
        {
            return _abilityCooldown;
        }
        set
        {
            _abilityCooldown = value;
        }
    }

    private string AbilityDescription
    {
        get
        {
            return _abilityDescription;
        }
        set
        {
            _abilityDescription = value;
        }
    }

    private float AbilityDuration
    {
        get
        {
            return _abilityDuration;
        }
        set
        {
            _abilityDuration = value;
        }
    }
    #endregion

    #region Methods

    virtual public void ActivateAbility(MonoBehaviour runner)
    {
        if (isOnCooldown)
        {
            return;
        }

        // Activate ability here
        runner.StartCoroutine(GoOnCooldown());
    }

    #endregion 

    private IEnumerator GoOnCooldown()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(_abilityCooldown);

        isOnCooldown = false;
    }
}
