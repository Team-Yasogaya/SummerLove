using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName {
    public class DialogueHistory : MonoBehaviour
    {
        public static DialogueHistory Instance;

        public class DialogueRecord
        {
            public Dialogue dialogue;
            public string talker;
            public List<DialogueNode.DialogueClue> collectedClues;
        }

        private List<DialogueRecord> _registeredDialogues;
        private Dictionary<Dialogue, DialogueRecord> _cachedDialogues;

        public List<DialogueRecord> RegisteredDialogues 
        { 
            get
            {
                if (_registeredDialogues == null)
                {
                    _registeredDialogues = new();
                }

                return _registeredDialogues;
            }
        }

        public Dictionary<Dialogue, DialogueRecord> CachedDialogues
        {
            get
            {
                if (_cachedDialogues == null)
                {
                    _cachedDialogues = new();
                }

                return _cachedDialogues;
            }
        }


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void AddDialogueToHistory(Dialogue dialogue, string talker)
        {
            if (GetRecordByDialogue(dialogue) != null)
            {
                Debug.Log("Double insert error of the same Dialogue: " + dialogue);
                return;
            }

            DialogueRecord newRecord = new DialogueRecord { dialogue = dialogue, talker = talker, collectedClues = new() };
            RegisteredDialogues.Add(newRecord);
        }

        public void AddCollectedClueToDialogue(Dialogue dialogue, DialogueNode.DialogueClue clue)
        {
            if (GetRecordByDialogue(dialogue) == null)
            {
                Debug.Log("Dialogue not found in history: " + dialogue);
                return;
            }

            if (GetRecordByDialogue(dialogue).collectedClues.Contains(clue))
            {
                Debug.Log("Dialogue Clue already collected for dialogue: " + dialogue);
                return;
            }

            GetRecordByDialogue(dialogue).collectedClues.Add(clue);
        }

        public void RemoveCollectedClueFromOwnerDialogue(DialogueNode.DialogueClue clue) 
        {
            foreach (var record in RegisteredDialogues) 
            {
                if (record.collectedClues.Contains(clue))
                {
                    record.collectedClues.Remove(clue);
                    return;
                }
            }

            Debug.Log("Clue not found in the history. This should not happens.");
        }

        public DialogueRecord GetRecordByDialogue(Dialogue dialogue)
        {
            if (CachedDialogues.ContainsKey(dialogue))
            {
                return CachedDialogues[dialogue];
            }

            foreach (var record in RegisteredDialogues)
            {
                if (record.dialogue.Equals(dialogue))
                {
                    CachedDialogues.Add(dialogue, record);
                    return record;
                }
            }

            return null;
        }
    }
}