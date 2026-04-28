using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class AbilityManager : MonoBehaviour
{
    private BaseAbility[] abilities = new BaseAbility[3];

    [Header("Lawyer Abilities")]
    [SerializeField] private BaseAbility pleaDeal;

    [Header("Nurse Abilities")]

    public static AbilityManager Instance;

    #region Methods
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

        //Debug.Log("This gameobject is: " + this.gameObject.GetComponent<LawyerScript>());

        if (this.gameObject.GetComponent<LawyerScript>() == null)
        {
            // Add Nurse Abilities to array
        } 
        else
        {
            Debug.Log("Adding lawyer abilities");
            
            abilities[0] = pleaDeal;
            // abilities[1] = ability1
        }
    }

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

    public void RemoveAbility(BaseAbility abilityToRemove)
    {
        for (int i = 0; i <  abilities.Length; i++)
        {
            if (abilities[i].Equals(abilityToRemove))
            {
                abilities[i] = null; 
            }
        }
    }

    public void OnAbility1(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Using ability 1");
            
            abilities[0].UseAbility(this);
        }
    }

    public void OnAbility2(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Using ability 2");

            abilities[1].UseAbility(this);
        }
    }

    public void OnAbility3(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Using ability 3");

            abilities[2].UseAbility(this);
        }
    }

    #endregion
}
