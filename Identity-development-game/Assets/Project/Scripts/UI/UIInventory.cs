using UnityEngine;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private GameObject ContentGrid;
    private List<ItemData> InventoryList;
    void Start(){
        InventoryList = GameManager.Instance._playerData.Inventory;
        ReloadInventory();
    }

    /// <summary>
    /// Reloads the inventory contents by looking at the players inventory
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
