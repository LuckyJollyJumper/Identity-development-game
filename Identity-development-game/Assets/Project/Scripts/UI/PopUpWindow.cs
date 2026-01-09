using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopUpWindow : MonoBehaviour, IPointerClickHandler
{
    private Playercontroller player;

    public void Start(){
        player = FindFirstObjectByType<Playercontroller>();
        player.StartInteraction();
    }

    public void OnPointerClick(PointerEventData eventData){
        player.EndInteraction();
        this.gameObject.SetActive(false);
    }

}
