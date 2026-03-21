using UnityEngine;

[CreateAssetMenu(fileName = "BaseAbility", menuName = "Scriptable Objects/BaseAbility")]
public class BaseAbility : ScriptableObject
{

    #region Properties

    //[SerializeField]
    //private string abilityName;

    [SerializeField]
    private float abilityCooldown;

    [SerializeField]
    private string abilityDescription;

    [SerializeField]
    private float abilityDuration;
    #endregion

    #region Getters and Setters

    private string abilityName
    {
        get { return abilityName.Trim(); }
        set { abilityName = value; }
    }
    #endregion

    #region Methods

    virtual public void ActivateAbility()
    {
        if (abilityCooldown <= 0)
        {
            // Do Something
            Debug.Log("Base Ability Activated");
        }
    }

    #endregion 
}
