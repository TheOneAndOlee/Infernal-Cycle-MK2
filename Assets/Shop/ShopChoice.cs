using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopChoice : MonoBehaviour
{
    public BaseItem item;

    [SerializeField]
    private TMPro.TMP_Text nameField;

    [SerializeField]
    private TMPro.TMP_Text descriptionField;

    [SerializeField]
    private TMPro.TMP_Text costField;

    [SerializeField]
    private Image thumbnail;

    private ItemManager itemManager;

    public void Setup(BaseItem baseItem, ItemManager manager)
    {
        item = baseItem;
        itemManager = manager;

        UpdateName();
        UpdateDescription();
        UpdateThumbnail();
        UpdateCost();

        // Important: Ensure the UI Button component calls OnClick. Unsubscribe first to avoid duplicate events.
        Button button = GetComponentInChildren<Button>();
        if (button != null)
        {
            button.onClick.RemoveListener(OnClick);
            button.onClick.AddListener(OnClick);
        }
    }

    private void UpdateName()
    {
        if (item != null && nameField != null)
        {
            nameField.text = item.ItemName;
        }
    }

    private void UpdateDescription()
    {
        if (item != null && descriptionField != null)
        {
            descriptionField.text = item.itemDescription;
        }
    }

    private void UpdateThumbnail()
    {
        if (item != null && thumbnail != null && item.image != null)
        {
            thumbnail.sprite = item.image.sprite;
            thumbnail.enabled = thumbnail.sprite != null;
        }
    }

    private void UpdateCost()
    {
        if (item != null && costField != null)
        {
            costField.text = "$" + (int)item.price;
        }
    }

    public void OnClick()
    {
        if (itemManager != null && item != null)
        {
            if (PlayerVariables.Instance != null && PlayerVariables.Instance.Money >= item.price)
            {
                PlayerVariables.Instance.Money -= item.price;
                itemManager.AddItem(item);

                GetComponent<Button>().interactable = false;

                Debug.Log("Purchased " +  item.name);
                // Optionally disable the button or visually mark it as sold out here
            }
            else
            {
                Debug.LogWarning("Not enough money for this item!");
            }
        }
    }
}
