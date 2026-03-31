using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private BaseAbility[] abilities;


    #region Methods
    public void AddAbility(BaseAbility ability)
    {
        // Add the ability to the abilities array
        // Implement once we have a way to manage the abilities array (e.g. List<BaseAbility>)
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
