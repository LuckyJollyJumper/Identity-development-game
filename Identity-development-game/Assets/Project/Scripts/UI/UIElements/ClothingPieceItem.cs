using UnityEngine;

public class ClothingPieceItem : DraggableItem
{
    public enum ClothingSize { Small, Medium, Large };
    public enum ClothingType { Shirt, Pants, Shoes };
    public enum ClothingColour{ Red, Blue, White };
    [Header("Clothing Piece Settings")]
    [SerializeField] public ClothingSize size;
    [SerializeField] public ClothingType type;

    private TMPro.TMP_Text ClothingLabel;

    public override void Start(){
        base.Start();
        ClothingLabel = this.GetComponentInChildren<TMPro.TMP_Text>();
    }

    public void SetClothing(ClothingType type, ClothingColour colour, ClothingSize size){
        SetClothingType(type);
        SetColour(colour);
        SetClothingSize(size);
    }

    private void SetClothingType(ClothingType type){
        switch (type){
            case ClothingType.Shirt:
                base.ItemImage.sprite = Resources.Load<Sprite>("Sprites/Top");
                break;
            case ClothingType.Pants:
                base.ItemImage.sprite = Resources.Load<Sprite>("Sprites/Bottom");
                break;
            case ClothingType.Shoes:
                base.ItemImage.sprite = Resources.Load<Sprite>("Sprites/Footwear");
                break;
        }
    }

    private void SetClothingSize(ClothingSize size){
        switch (size){
            case ClothingSize.Small:
                ClothingLabel.text = "S";
                break;
            case ClothingSize.Medium:
                ClothingLabel.text = "M";
                break;
            case ClothingSize.Large:
                ClothingLabel.text = "L";
                break;
        }
    }

    private void SetColour(ClothingColour colour){
        switch (colour){
            case ClothingColour.Blue:
                base.ItemImage.color = Color.blue;
                break;
            case ClothingColour.Red:
                base.ItemImage.color = Color.red;
                break;
            case ClothingColour.White:
                base.ItemImage.color = Color.white;
                break;
        }
    }

}
