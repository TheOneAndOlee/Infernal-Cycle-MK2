using System;
using System.Collections;
using UnityEngine;

//[CreateAssetMenu(fileName = "BaseItem", menuName = "Scriptable Objects/BaseItem")]
public abstract class BaseItem : ScriptableObject
{
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum ItemUseType
    {
        Passive,
        Active
    }

    public enum ItemClass
    {
        Defense,
        Offense,
        Support
    }

    [SerializeField]
    private string itemName;
    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    [TextArea]
    public string itemDescription;

    [SerializeField] private ItemRarity itemRarity;
    [SerializeField] private ItemUseType itemUseType;
    [SerializeField] private ItemClass itemClass;

    public ItemRarity Rarity => itemRarity;
    public ItemUseType UseType => itemUseType;
    public ItemClass Classification => itemClass;

    /// <summary>
    /// Runs a 1-time effect upon item usage
    /// Overriden when inherited
    /// </summary>
    public virtual void UseItem(ItemManager manager)
    {
        Debug.Log("Using item: " + itemName);
    }

    /// <summary>
    /// A coroutine for continuous item effects (heal over time, temporary boosts, etc.)
    /// Also overridden as needed
    /// </summary>
    public virtual IEnumerator PassiveItemEffect(ItemManager manager, int stacks)
    {
        yield break;
    }

}
