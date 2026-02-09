using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class that handles the UI of the inventory
/// </summary>
public class UIInventory : MonoBehaviour
{
    [SerializeField] private GameObject ContentGrid; //Inventory item grid
    private List<ItemData> InventoryList; // Reference to all items in the players inventory
    [Header("Debug")]
    [SerializeField] private bool DebugMode = false;
    private string DebugID = "[UI Inventory]";
    void Start(){
        InventoryList = GameManager.Instance._playerData.Inventory;
    }

    /// <summary>
    /// Reloads the inventory contents by looking at the players inventory in GameManager. Is also called from the UIMainProfile
    /// </summary>
    public void ReloadInventory(){
        foreach (Transform child in ContentGrid.transform){
            Destroy(child.gameObject);
        }

        this.InventoryList = GameManager.Instance._playerData.Inventory;

        foreach (ItemData shopItem in InventoryList){
            GameObject shopItemPrefab = Resources.Load<GameObject>("InventorySlot");
            shopItemPrefab.GetComponent<InventorySlot>().SetInventorySlot(shopItem);
            Instantiate(shopItemPrefab, ContentGrid.transform);
            if(DebugMode){Debug.Log($"{DebugID} Loaded {shopItem.ItemName} in inventory");}
        }
    }
}
