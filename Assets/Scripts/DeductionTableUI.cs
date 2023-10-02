using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName {
    public class DeductionTableUI : MonoBehaviour
    {
        private void RemoveClueFromDeductionTableConfirmation(DialogueClue clue) 
        {
            GameUI.ConfirmationModal.Show("Are you sure you want to remove this clue?");
            GameUI.ConfirmationModal.OnConfirm += () => RemoveClueFromDeductionTable(clue);
        }

        private void RemoveClueFromDeductionTable(DialogueClue clue) 
        {
            DialogueHistory.Instance.RemoveCollectedClueFromOwnerDialogue(clue);
        }
    }
}