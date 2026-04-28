using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private PlayerMovement playerMovement;

    public PlayerVariables Variables => PlayerVariables.Instance;
    public PlayerMovement Movement => playerMovement;

    [SerializeField] private BaseItem testItem;
    private Dictionary<BaseItem, int> items = new Dictionary<BaseItem, int>();

    private Dictionary<BaseItem, Coroutine> activeCoroutines = new Dictionary<BaseItem, Coroutine>();

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
    }

    public void AddItem(BaseItem item)
    {
        Debug.Log("Adding item: " + item.name);
        if (items.TryGetValue(item, out int count))
        {
            items[item]++;
        }
        else
        {
            items[item] = 1;
        }

        item.UseItem(this);

        RefreshContinuousEffects(item);
    }

    public void SubtractItem(BaseItem item, int subtractor)
    {
        if (items.TryGetValue(item, out int count))
        {
            Debug.Log("Subtracting " + subtractor + " " + item.name + "(s)");
            items[item] -= subtractor;
            
            if (items[item] <= 0)
            {
                items.Remove(item);

                if (activeCoroutines.ContainsKey(item))
                {
                    StopCoroutine(activeCoroutines[item]);
                    activeCoroutines.Remove(item);
                }
            } 
            else
            {
                RefreshContinuousEffects(item);
            }
        }
        else
        {
            Debug.LogWarning("Item: " + item.ItemName + " could not be subtracted because it does not exist in the inventory.");
        }
    }

    private void RefreshContinuousEffects(BaseItem item)
    {
        if (activeCoroutines.ContainsKey(item) && activeCoroutines[item] != null)
        {
            StopCoroutine(activeCoroutines[item]);
        }

        Coroutine newCoroutine = StartCoroutine(item.PassiveItemEffect(this, items[item]));
        activeCoroutines[item] = newCoroutine;
    }

    public void TriggerEnemyDamagedEffects(GameObject enemy)
    {
        foreach (var itemPair in items)
        {
            BaseItem item = itemPair.Key;

            item.OnEnemyDamaged(enemy, this);
        }
    }
}
