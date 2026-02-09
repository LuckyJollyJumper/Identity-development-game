using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class that handles the UI of the inventory
/// </summary>
public class UIInventory : MonoBehaviour
{
    [SerializeField] private GameObject ContentGrid; //Inventory item grid
    private List<ItemData> InventoryList; // Reference to all items in the players inventory
    void Start(){
        InventoryList = GameManager.Instance._playerData.Inventory;
        ReloadInventory();
    }

    /// <summary>
    /// Reloads the inventory contents by looking at the players inventory in GameManager
    /// </summary>
    public void ReloadInventory(){
        this.InventoryList = GameManager.Instance._playerData.Inventory;

        foreach (ItemData shopItem in InventoryList){
            GameObject shopItemPrefab = Resources.Load<GameObject>("InventorySlot");
            shopItemPrefab.GetComponent<InventorySlot>().SetInventorySlot(shopItem);
            Instantiate(shopItemPrefab, ContentGrid.transform);
        }
    }
}
