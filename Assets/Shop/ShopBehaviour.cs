using System;
using UnityEngine;
using static UnityEditor.Progress;

public class ShopBehaviour : MonoBehaviour
{
    public static ShopBehaviour Instance { get; private set; }

    [SerializeField] private GameObject shopChoicePrefab;
    private ItemManager itemManager => ItemManager.Instance;
    
    [SerializeField] private GameObject shopCamera; // Virtual Camera for the shop

    [Header("Internal Component References")]
    [SerializeField] private ItemList itemList;
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private RectTransform choicesContainer; // Parent for the instantiated choices
    [SerializeField] private TMPro.TMP_Text moneyField;
    [SerializeField] private TMPro.TMP_Text refreshCostField;

    [Header("Internal Variables")]
    [SerializeField] private float baseRefreshCost = 75f;
    private float currentRefreshCost;

    public bool isActive = false;

    public int numItemChoices = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (PlayerVariables.Instance != null)
        {
            PlayerVariables.Instance.OnMoneyChanged.AddListener(UpdateMoneyField);
        }
        UpdateMoneyField();
    }

    private void OnDestroy()
    {
        if (PlayerVariables.Instance != null)
        {
            PlayerVariables.Instance.OnMoneyChanged.RemoveListener(UpdateMoneyField);
        }
    }

    public void OpenItemShop()
    {
        isActive = true;
        EnableShopMenu();

        if (itemList == null || shopChoicePrefab == null || choicesContainer == null || itemManager == null)
        {
            Debug.LogWarning("ShopBehaviour is missing required references (itemList, shopChoicePrefab, choicesContainer, or itemManager).");
            return;
        }

        currentRefreshCost = baseRefreshCost;

        PopulateChoices();
    }

    private void PopulateChoices()
    {
        if (choicesContainer != null)
        {
            foreach (Transform child in choicesContainer)
            {
                Destroy(child.gameObject);
            }
        }

        BaseItem[] choices = itemList.RandomNItems(numItemChoices);

        foreach (BaseItem item in choices)
        {
            if (item != null)
            {
                GameObject choiceObj = Instantiate(shopChoicePrefab);
                choiceObj.transform.SetParent(choicesContainer, false);
                choiceObj.transform.SetAsLastSibling();
                Debug.Log("Instantiating shop choice.");

                RectTransform rectTransform = choiceObj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.localScale = Vector3.one;
                    rectTransform.anchoredPosition3D = Vector3.zero;
                }

                ShopChoice shopChoice = choiceObj.GetComponent<ShopChoice>();
                if (shopChoice != null)
                {
                    shopChoice.Setup(item, itemManager);
                }
            }
        }
    }

    public void RefreshShop()
    {
        if (PlayerVariables.Instance != null && PlayerVariables.Instance.Money >= currentRefreshCost) 
        {
            PlayerVariables.Instance.Money -= currentRefreshCost;

            currentRefreshCost *= 2;

            refreshCostField.text = "Refresh Options - $" + currentRefreshCost;

            PopulateChoices();
        }
        else
        {
            Debug.LogWarning("Can't refresh shop, not enough money");
        }
    }

    public void EnableShopMenu()
    {
        if (shopCanvas)
        {
            shopCanvas.SetActive(true);
        }
        if (shopCamera)
        {
            shopCamera.SetActive(true);
        }

        isActive = true;

        Time.timeScale = 0f;
    }

    public void DisableShopMenu()
    {
        if (shopCanvas)
        {
            shopCanvas.SetActive(false);
        }
        if (shopCamera)
        {
            shopCamera.SetActive(false);
        }

        isActive = false;

        Time.timeScale = 1f;
    }

    public void ToggleShopMenu()
    {
        if (shopCanvas)
        {
            shopCanvas.SetActive(!shopCanvas.activeSelf);
        }
        if (shopCamera)
        {
            shopCamera.SetActive(!shopCamera.activeSelf);
        }

        isActive = shopCanvas != null && shopCanvas.activeSelf;
        Time.timeScale = isActive ? 0f : 1f;

    }

    private void UpdateMoneyField()
    {
        if (PlayerVariables.Instance != null && moneyField != null)
        {
            moneyField.text = "Money: " + PlayerVariables.Instance.Money.ToString();
        }
    }
}
