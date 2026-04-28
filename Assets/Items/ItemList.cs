using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemList : MonoBehaviour
{
    [SerializeField] private List<BaseItem> items = new List<BaseItem>();

    [System.Serializable]
    private struct RarityWeightEntry
    {
        public BaseItem.ItemRarity rarity;
        [Min(0f)] public float weight;
    }

    [SerializeField]
    private List<RarityWeightEntry> rarityWeightTable = new()
    {
        new RarityWeightEntry { rarity = BaseItem.ItemRarity.Common,    weight = 0.5f },
        new RarityWeightEntry { rarity = BaseItem.ItemRarity.Uncommon,  weight = 0.25f },
        new RarityWeightEntry { rarity = BaseItem.ItemRarity.Rare,      weight = 0.15f },
        new RarityWeightEntry { rarity = BaseItem.ItemRarity.Epic,      weight = 0.07f },
        new RarityWeightEntry { rarity = BaseItem.ItemRarity.Legendary, weight = 0.03f },
    };

    private Dictionary<BaseItem.ItemRarity, float> weights = new();

    public Dictionary<BaseItem.ItemRarity, List<BaseItem>> ItemDict { get; private set; } = new Dictionary<BaseItem.ItemRarity, List<BaseItem>>();

    private void Awake()
    {
        BuildDictionary();
        BuildWeights();

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

    private void BuildWeights()
    {
        weights.Clear();

        foreach (var entry in rarityWeightTable)
        {
            float clamped = Mathf.Max(0f, entry.weight);

            // Last one wins if duplicate rarity appears in the table
            weights[entry.rarity] = clamped;
        }
    }

    public BaseItem[] RandomNItems(int n)
    {
        // n Less than or Equal to 0 check
        if (n <= 0) return new BaseItem[0];

        // If items don't exist, return an empty array
        if (items == null || items.Count == 0) return new BaseItem[n];

        // If we don't have our item dictionary, build it
        if (ItemDict == null || ItemDict.Count == 0) BuildDictionary();
        
        // If we dont have weights, build them
        if (weights == null || weights.Count == 0) BuildWeights();


        // Build the list of available rarities in case any are unavailable
        List<BaseItem.ItemRarity> availableRarities = new();
        float totalWeight = 0f;

        foreach (var kvp in ItemDict)
        {
            if (kvp.Value == null || kvp.Value.Count == 0)
            {
                continue;
            }

            if (!weights.TryGetValue(kvp.Key, out float w))
            {
                continue;
            }

            if (w <= 0f)
            {
                continue;
            }

            availableRarities.Add(kvp.Key);
            totalWeight += w;
        }

        BaseItem[] output = new BaseItem[n];
        List<BaseItem> nonNullItems = items.Where(i => i != null).ToList();
        
        if (availableRarities.Count == 0 || totalWeight <= 0f)
        {
            for (int i = 0; i < n; i++)
            {
                output[i] = nonNullItems.Count > 0
                    ? nonNullItems[Random.Range(0, nonNullItems.Count)]
                    : null;
            }

            return output;
        }

        for (int i = 0; i < n; i++)
        {
            float roll = Random.value * totalWeight;
            float cumulative = 0f;

            BaseItem.ItemRarity chosenRarity = availableRarities[availableRarities.Count - 1];

            for (int r = 0; r < availableRarities.Count; r++)
            {
                BaseItem.ItemRarity rarity = availableRarities[r];
                cumulative += weights[rarity];

                if (roll <= cumulative)
                {
                    chosenRarity = rarity;
                    break;
                }
            }

            List<BaseItem> pool = ItemDict[chosenRarity];
            output[i] = pool[Random.Range(0, pool.Count)];
        }

        return output;
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
        BuildWeights();
    }
#endif
}
