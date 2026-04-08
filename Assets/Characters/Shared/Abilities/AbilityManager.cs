using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private BaseAbility[] abilities;


    #region Methods
    public void UpdateAbilities(BaseAbility ability, int slot)
    {
        // Add the ability to the abilities array
        // Implement once we have a way to manage the abilities array (e.g. List<BaseAbility>)
        if (abilities[slot] == null)
        {
            abilities[slot] = ability;
            Debug.Log("Added ability: " + ability.name + " to slot: " + slot);
        } 
        else
        {
            Debug.Log("Replaced ability " + abilities[slot] + " in slot: " + slot + " with ability: " + ability);
            abilities[slot] = ability;
        }
    }

    public void RemoveAbility(BaseAbility ability)
    {

    }

    public void OnAbility1()
    {
        abilities[0].ActivateAbility(this);
    }

    public void OnAbility2()
    {
        abilities[1].ActivateAbility(this);
    }

    public void OnAbility3()
    {
        abilities[2].ActivateAbility(this);
    }

    #endregion
}
