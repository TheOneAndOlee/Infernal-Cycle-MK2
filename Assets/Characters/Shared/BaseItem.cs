using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]
public class NewScriptableObjectScript : ScriptableObject
{
    [SerializeField]
    private string itemName;
    private int count;

    // Reference to the Character so that we can parent the item to the itemManager when obtained
    // Implement once Character and ItemManager are implemented
    //private Character character; 

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    void Start()
    {

    }

}
