using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Class that controls all the UI elements in the shop such as buying of items and filling the shops
/// </summary>
public class UIShop : MonoBehaviour
{
    [HideInInspector] private InteractableObject Parent; // Used to close the UI via the interactableObject
    [Header("References")]
    [SerializeField] private GameObject Grid; // UI grid that holds the items in the shop
    private List<ItemData> ShopInventory; // The data for all the shopItems

    [Header("Debug")]
    [SerializeField ] public bool DebugMode = false;
    private string DebugID = "[Shop UI]";

    void Start(){
        this.Parent = transform.parent.GetComponent<InteractableObject>();

        ShopInventory = new(){
            new ItemData(){
                ItemName = "Item1",
                CoinCost = 10,
                Description = "Item1"
            },
            new ItemData(){
                ItemName = "Item2",
                CoinCost = 5,
                Description = "Item2"
            },
            new ItemData(){
                ItemName = "Item3",
                CoinCost = 3,
                Description = "Item3"
            },
            new ItemData(){
                ItemName = "Item4",
                CoinCost = 0,
                Description = "Item4"
            },

        };

        ReloadShop();
    }

    /// <summary>
    /// Reloads the shop contents by looking at the players inventory and loading in the rest of the ShopInventory List
    /// </summary>
    public void ReloadShop(){
        foreach (Transform child in Grid.transform){
            Destroy(child.gameObject);
        }
        
        List<ItemData> inv = GameManager.Instance._playerData.Inventory;
        // Use of Any because Contains does not use the overriden Equals in ItemData
        ShopInventory.RemoveAll(item => inv.Any(i => i.Equals(item)));

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
        if (shopItem.CoinCost <= GameManager.Instance._playerData.Coins){
            if(DebugMode){ Debug.Log($"{DebugID} Buying {shopItem} for {shopItem.CoinCost}"); }
            GameManager.Instance.AddInventoryItem(shopItem);
            ReloadShop();
        }
    }

    /// <summary>
    /// Used by the QuitButton to close the UI again. Calls InteractableObject method
    /// </summary>
    public void CloseUI(){
        ReloadShop(); // Just for testing purposes, can be removed.
        Parent.OnEndInteract();
    }
}
