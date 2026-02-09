using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    [HideInInspector] public UIShop Parent; // Event triggered when the pop-up is closed
    [SerializeField] private TMPro.TextMeshProUGUI CostText;
    [SerializeField] private TMPro.TextMeshProUGUI NameText;
    [SerializeField] private Image ItemImage;
    [SerializeField] private GameObject ConfirmationPopUp;
    private ItemData ShopItem;

    void Start(){
        ConfirmationPopUp.SetActive(false);
    }

    public void SetShopItemSlot(UIShop parent, ItemData shopItem){
        this.Parent = parent;
        this.ShopItem = shopItem;

        this.CostText.text = $"{ShopItem.CoinCost}<Sprite index=0>";
        this.ItemImage.sprite = ShopItem.Sprite;
        this.NameText.text = ShopItem.ItemName;
    }

    /// <summary>
    /// Called by this item to start the buying process higher in the hirarchy from the Confirmation popup
    /// </summary>
    public void CanBuyItem(){
        Parent.CanBuyItem(ShopItem);
        CloseConfirmationPopUp();
    }
    public void CloseConfirmationPopUp(){ ConfirmationPopUp.SetActive(false); }
    public void OpenConfirmationPopUp(){ ConfirmationPopUp.SetActive(true); }

    public void DestroyObject(){
        this.gameObject.SetActive(false);
        Destroy(this);
    }
}
