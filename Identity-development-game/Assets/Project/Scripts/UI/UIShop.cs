using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    [HideInInspector] private InteractableObject Parent;
    [SerializeField] private GameObject Grid;
    public List<ItemData> ShopInventory; // The data for all the shopItems
    void Start(){
        ShopInventory = new(){
            new ItemData(){
                ItemName = "Item1",
                CoinCost = 10,
            },
            new ItemData(){
                ItemName = "Item1",
                CoinCost = 5,
            },
            new ItemData(){
                ItemName = "Item1",
                CoinCost = 3,
            },
        };

        ReloadShop();
    }

    /// <summary>
    /// Reloads the shop contents by looking at the players inventory and loading in the rest of the ShopInventory List
    /// </summary>
    public void ReloadShop(){
        List<ItemData> inv = GameManager.Instance._playerData.Inventory;
        ShopInventory.RemoveAll(item => inv.Contains(item));

        foreach (ItemData shopItem in ShopInventory){
            GameObject shopItemPrefab = Resources.Load<GameObject>("ShopSlot");
            shopItemPrefab.GetComponent<ShopItemSlot>().SetShopItemSlot(this, shopItem);
            Instantiate(shopItemPrefab, Grid.transform);
        }
    }

    /// <summary>
    /// Checks whether the player has enough coins to buy the item. If so it will update the players inventory and
    /// coins and will update the shop.
    /// </summary>
    /// <param name="shopItem"></param>
    public void CanBuyItem(ItemData shopItem){
        if (shopItem.CoinCost >= GameManager.Instance._playerData.Coins){
            GameManager.Instance._playerData.Inventory.Add(shopItem);
            GameManager.Instance._playerData.Coins -= shopItem.CoinCost;

            ReloadShop();
        }
    }

    /// <summary>
    /// Used by the QuitButton to close the UI again. Calls InteractableObject method
    /// </summary>
    public void CloseUI(){
        Parent.OnEndInteract();
    }
}
