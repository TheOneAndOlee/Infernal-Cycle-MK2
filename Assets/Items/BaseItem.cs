using System;
using System.Collections;
using UnityEngine;

//[CreateAssetMenu(fileName = "BaseItem", menuName = "Scriptable Objects/BaseItem")]
public abstract class BaseItem : ScriptableObject
{
    [SerializeField]
    private string itemName;
    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    [TextArea]
    public string itemDescription;

    /// <summary>
    /// Runs a 1-time effect upon item usage
    /// Overriden when inherited
    /// </summary>
    /// <param name="manager"></param>
    public virtual void UseItem(ItemManager manager)
    {
        Debug.Log("Using item: " + itemName);
    }

    /// <summary>
    /// A coroutine for continuous item effects (heal over time, temporary boosts, etc.)
    /// Also overridden as needed
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator PassiveItemEffect(ItemManager manager, int stacks)
    {
        yield break;
    }

}
