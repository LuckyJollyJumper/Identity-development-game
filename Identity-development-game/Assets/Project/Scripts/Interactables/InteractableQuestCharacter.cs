using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// InteractableObject for NPC characters that provide a quest to the player. After the quest is completed
/// the NPC can have different dialogue and 
/// </summary>
public class InteractableQuestCharacter : InteractableCharacter
{
    [Header("Quest")]
    [SerializeField] QuestData NPCQuest; // The quest that the NPC gives, used to signal the questmanager
    [TextArea][SerializeField] List<string> QuestCompletedTexts; // The text that the NPC will say when interacting with it after you completed the quest, can be empty
    [TextArea][SerializeField] List<string> PostQuestTexts; // The text that the NPC will say when interacting with it after you already completed the quest, can be empty
   
    /// <summary>
    /// Sets the quest as active in the questmanager when the pop-up is closed on first interaction with the NPC.
    /// </summary>
    public override void Start(){
        this.NPCQuest = new(){
            QuestName = "Verloren boeken",
            Description = "Vind alle boeken voor Axel die verspreid liggen in de school",
            Goal = 7,
            CurrentProgress = 0,
            RewardCoins = 1,
            RewardPoints = 400,
            IsActive = false
        };
        // This is the callback that will be called when the quest is completed.
        NPCQuest.OnQuestCompleted += () => { QuestCompleted(NPCQuest); };
        QuestManager.Instance.AddQuestData(NPCQuest, this);

        // Prime the event to start the quest at the end of the NPC interaction
        base.UI.GetComponent<PopUpWindowCharacter>().OnPopUpClosed += () => {
            QuestManager.Instance.SetQuestAsActive(NPCQuest.QuestName);
            if (DebugMode){ Debug.Log($"{DebugID} OnPopUpClosed event triggered from PopUpWindowCharacter, setting quest {NPCQuest.QuestName} as active in QuestManager"); }
        };
        base.Start();
    }

    /// <summary>
    /// Called by the questManager when the quest is completed. Changes the NPC's dialogue to reflect the completion of the quest the next time
    /// the player interacts with it. It will also prime the NPC to have a new dialogue when having interacted with the QuestCompletedText that 
    /// will stay for every interaction afterwards.
    /// </summary>
    public void QuestCompleted(QuestData quest){
        if (DebugMode){ Debug.Log($"{DebugID} Quest {NPCQuest.QuestName} completed!"); }
        PopUpWindowCharacter popUpWindowCharacter = base.UI.GetComponent<PopUpWindowCharacter>();
        
        popUpWindowCharacter.ClearActions(); // Clear the previous event
        popUpWindowCharacter.SetCharacterPopUpText(QuestCompletedTexts, CharacterName);
        popUpWindowCharacter.OnPopUpClosed += () => {
            GameManager.Instance.AddQuestRewards(quest.RewardCoins, quest.RewardPoints);
            // Set the post quest completion texts and clear all possible calls
            popUpWindowCharacter.SetCharacterPopUpText(PostQuestTexts, CharacterName);
            popUpWindowCharacter.ClearActions(); // Clear the event to prevent it from being called multiple times
        };
    }
    
}
