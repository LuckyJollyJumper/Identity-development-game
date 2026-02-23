using UnityEngine;

/// <summary>
/// Class for interactbale objects that are conected to a quest.
/// </summary>
public class InteractableHiddenObject: InteractableObject
{
    [Header("Hidden Object")]
    public string QuestName = "Verloren boeken";
    public string PlayerName = "Axel";

    public override void Start(){
        this.InteractionObject.GetComponentInChildren<TextBubble>().SetBubbleText(PopUpText, 20);
        this.InteractionObject.SetActive(false);
        this.UI.SetActive(false);
        this.DebugID = $"[InteractableObject/{this.gameObject.name}]";
    }

    public override void OnInteract(Playercontroller player){
        QuestData currentObjectQuest = QuestManager.Instance.GetQuestData(QuestName);

        if (currentObjectQuest.IsActive){
            if (currentObjectQuest.IsCompleted()){
                base.UI.GetComponent<PopUpWindow>().SetPopUpText($"Je hebt alle boeken gevonden!\nBreng ze terug naar {PlayerName}!");
            }
            else{
                base.UI.GetComponent<PopUpWindow>().SetPopUpText($"Je hebt een boek gevonden!\nNog {currentObjectQuest.Goal - currentObjectQuest.CurrentProgress} te gaan!");
            }
        }
        else if (currentObjectQuest.CurrentProgress == 0){
            base.UI.GetComponent<PopUpWindow>().SetPopUpText($"Je hebt een boek gevonden!\nWie zou deze verloren hebben?");
        } else{
            base.UI.GetComponent<PopUpWindow>().SetPopUpText($"Je hebt nog een boek gevonden!\nJe hebt er al {currentObjectQuest.CurrentProgress} van de {currentObjectQuest.Goal} boeken gevonden!\nMaar wie zou er nou zoveel boeken verloren hebben?");
        }
        QuestManager.Instance.FoundHiddenObject(QuestName);

        base.OnInteract(player);
        Destroy(this.gameObject);
    }
}
