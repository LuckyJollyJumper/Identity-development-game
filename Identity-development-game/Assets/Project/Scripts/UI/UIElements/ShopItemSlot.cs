using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that handles a single item in the shop and can be interacted with.
/// </summary>
public class ShopItemSlot : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public UIShop Parent; // Event triggered when the pop-up is closed
    [SerializeField] private TMPro.TextMeshProUGUI CostText;
    [SerializeField] private TMPro.TextMeshProUGUI NameText;
    [SerializeField] private Image ItemImage;
    [SerializeField] private GameObject ConfirmationPopUp;
    [SerializeField] private TMPro.TextMeshProUGUI ConfirmationPopUpText;
    private ItemData ShopItem;

    void Start(){
        ConfirmationPopUp.SetActive(false);
    }

    /// <summary>
    /// Should be called when creating a new ShopItemSlot Instance to fill all its variables
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="shopItem"></param>
    public void SetShopItemSlot(UIShop parent, ItemData shopItem){
        this.Parent = parent;
        this.ShopItem = shopItem;

        this.CostText.text = $"{ShopItem.CoinCost}<Sprite index=0>";
        this.ItemImage.sprite = ShopItem.Sprite;
        this.NameText.text = ShopItem.ItemName;

        //GameObject.Find($"{ConfirmationPopUp.name}/MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = $"Weet je zeker dat je {this.ShopItem.CoinCost}<Sprite index=0> wilt betalen?";
        ConfirmationPopUpText.text =$"Weet je zeker dat je {this.ShopItem.CoinCost}<Sprite index=0> wilt betalen?";
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
}
