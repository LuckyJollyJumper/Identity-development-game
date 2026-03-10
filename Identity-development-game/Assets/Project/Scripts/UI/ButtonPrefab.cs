using UnityEngine;
using System;
using TMPro;

public class ButtonPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ButtonText;
    [HideInInspector] public event Action OnButtonPressed; // Event triggered when the pop-up is closed

    public void SetText(string bText){
        ButtonText.text = bText;
    }
    public void ButtonPressed(){
        OnButtonPressed?.Invoke();
    }
}
