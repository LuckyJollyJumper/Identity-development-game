using UnityEngine;

public class ClothingPieceItem : DraggableItem
{
     public enum ClothingSize { Small, Medium, Large };
    public enum ClothingType { Shirt, Pants, Shoes };
    [Header("Clothing Piece Settings")]
    [SerializeField] public ClothingSize size;
    [SerializeField] public ClothingType type;

    public override void Start(){
        base.Start();
        if (DebugMode){
            this.GetComponentInChildren<TMPro.TMP_Text>().text = $"{size}:{type}";
        }else{
            this.GetComponentInChildren<TMPro.TMP_Text>().text = "";
        }
    }

}
