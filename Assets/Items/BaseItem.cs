using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    public Image image;

    public float price;
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

    /// <summary>
    /// A overrideable function to trigger on-hit effects
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="manager"></param>
    public virtual void OnEnemyDamaged(GameObject enemy, ItemManager manager)
    {

    }
}
