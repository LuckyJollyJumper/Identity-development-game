using UnityEngine;

/// <summary>
/// Class used to store items that the player can buy.
/// </summary>
[System.Serializable]
public class ItemData
{
    public string ItemName;
    public string Description;
    public Sprite Sprite = null;
    public int CoinCost;

    /// <summary>
    /// Used to easily compare 2 items. Used for example in the shop. Ignores the sprite for now
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj){
        if (obj == null || GetType() != obj.GetType())
            return false;

        ItemData other = (ItemData)obj;
        return ItemName == other.ItemName 
                && Description == other.Description  
                && CoinCost == other.CoinCost;
    }public override int GetHashCode(){ return (ItemName, CoinCost).GetHashCode(); }

}
