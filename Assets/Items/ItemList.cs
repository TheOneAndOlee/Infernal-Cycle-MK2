using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemList : MonoBehaviour
{
    [SerializeField] private List<BaseItem> items = new List<BaseItem>();
    private Dictionary<BaseItem.ItemRarity, float> weights = new Dictionary<BaseItem.ItemRarity, float>
    {
        { BaseItem.ItemRarity.Common, 0.5f },
        { BaseItem.ItemRarity.Uncommon, 0.25f },
        { BaseItem.ItemRarity.Rare, 0.15f },
        { BaseItem.ItemRarity.Epic, 0.07f },
        { BaseItem.ItemRarity.Legendary, 0.03f }
    };

    public Dictionary<BaseItem.ItemRarity, List<BaseItem>> ItemDict { get; private set; } = new Dictionary<BaseItem.ItemRarity, List<BaseItem>>();

    private void Awake()
    {
        BuildDictionary();

        foreach (var item in items)
        {
            if (item != null)
            {
                Debug.Log($"Loaded item: {item.ItemName} with rarity {item.Rarity}");
            }
        }
    }

    private void BuildDictionary()
    {
        ItemDict = items
            .Where(item => item != null)
            .GroupBy(item => item.Rarity)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string[] guids = AssetDatabase.FindAssets("t:BaseItem");
        items = guids
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(path => AssetDatabase.LoadAssetAtPath<BaseItem>(path))
            .Where(item => item != null)
            .ToList();

        BuildDictionary();
    }
#endif
}
