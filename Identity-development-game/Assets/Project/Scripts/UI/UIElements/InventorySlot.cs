using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that is connected to the InventorySlot prefab and handles a single UI item object 
/// </summary>
public class InventorySlot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMPro.TextMeshProUGUI NameText;
    [SerializeField] private Image ItemImage;
    private ItemData ShopItem;

    /// <summary>
    /// Should be called when creating a new prefab instance. Sets all the internal values needed
    /// </summary>
    /// <param name="shopItem"></param>
    public void SetInventorySlot(ItemData shopItem){
        this.ShopItem = shopItem;

        this.ItemImage.sprite = ShopItem.Sprite;
        this.NameText.text = ShopItem.ItemName;
    }
}
