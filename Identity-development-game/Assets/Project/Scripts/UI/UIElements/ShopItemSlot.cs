using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    [HideInInspector] public UIShop Parent; // Event triggered when the pop-up is closed
    [SerializeField] private TMPro.TextMeshProUGUI CostText;
    [SerializeField] private TMPro.TextMeshProUGUI NameText;
    [SerializeField] private Image ItemImage;
    [SerializeField] private GameObject ConfirmationPopUp;
    [SerializeField] private TMPro.TextMeshProUGUI ConfirmationPopUpText;
    public ItemData ShopItem;

    void Start(){
        ConfirmationPopUp.SetActive(false);
    }

    public void SetShopItemSlot(UIShop parent, ItemData shopItem){
        Debug.Log($"New item set with: {shopItem.ItemName}");
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
        Debug.Log($"Checking if item {ShopItem.ItemName} can be purchased");
        Parent.CanBuyItem(ShopItem);
        CloseConfirmationPopUp();
    }
    public void CloseConfirmationPopUp(){ ConfirmationPopUp.SetActive(false); }
    public void OpenConfirmationPopUp(){ ConfirmationPopUp.SetActive(true); }
}
