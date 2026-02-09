using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI NameText;
    [SerializeField] private Image ItemImage;
    private ItemData ShopItem;

    public void SetInventorySlot(ItemData shopItem){
        this.ShopItem = shopItem;
        
        this.ItemImage.sprite = ShopItem.Sprite;
        this.NameText.text = ShopItem.ItemName;
    }
}
