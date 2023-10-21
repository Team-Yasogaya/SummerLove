using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoName {
    public class DialogueHistory : MonoBehaviour
    {
        public static DialogueHistory Instance;

        [System.Serializable]
        public class DialogueRecord
        {
            public Dialogue dialogue;
            public Talker talker;
            public List<DialogueNode.DialogueClue> collectedClues;

            //public bool AllCluesCollected
            //{
            //    get
            //    {
            //        return collectedClues.
            //    }
            //}
        }

        [SerializeField] private List<DialogueRecord> _registeredDialogues;

        private Dictionary<Dialogue, DialogueRecord> _cachedDialogues;
        private List<Talker> _talkerSupportList;

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

        public void AddDialogueToHistory(Dialogue dialogue, Talker talker)
        {
            if (GetRecordByDialogue(dialogue) != null)
            {
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
            Debug.Log("Remove Collected Clue Invoked");

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

        public IEnumerable<DialogueRecord> GetRecordsByTalker(Talker talker)
        {
            foreach (var record in RegisteredDialogues)
            {
                if (record.talker.Equals(talker))
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<Talker> GetAllRegisteredTalkers()
        {
            if (_talkerSupportList == null)
            {
                _talkerSupportList = new();
            }

            _talkerSupportList.Clear();

            foreach (var record in RegisteredDialogues)
            {
                if (_talkerSupportList.Contains(record.talker)) continue;

                _talkerSupportList.Add(record.talker);
                yield return record.talker;
            }
        }
    }
}