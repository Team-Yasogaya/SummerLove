using System.Collections.Generic;
using System.Linq;
using GameDevTV.Saving;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NoName 
{
    [System.Serializable]
    public class DialogueRecord
    {
        public Dialogue dialogue;
        public List<Clue> collectedClues;
    }
    
    public class DialogueHistory : MonoBehaviour, IJsonSaveable
    {
        public static DialogueHistory Instance;

        [SerializeField] List<DialogueRecord> _registeredDialogues;

        private Dictionary<Dialogue, DialogueRecord> cachedDialogues;
        private List<Npc> talkerSupportList;

        public List<DialogueRecord> RegisteredDialogues
        {
            get
            {
                _registeredDialogues ??= new();

                return _registeredDialogues;
            }
        }

        public Dictionary<Dialogue, DialogueRecord> CachedDialogues
        {
            get
            {
                cachedDialogues ??= new();

                return cachedDialogues;
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

        public void AddDialogueToHistory(Dialogue dialogue)
        {
            if (GetRecordByDialogue(dialogue) != null)
            {
                return;
            }

            DialogueRecord newRecord = new() { dialogue = dialogue, collectedClues = new() };
            RegisteredDialogues.Add(newRecord);
        }

        public void AddCollectedClueToDialogue(Dialogue dialogue, Clue clue)
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

        public void RemoveCollectedClueFromOwnerDialogue(Clue clue)
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

        public IEnumerable<DialogueRecord> GetRecordsByTalker(Npc talkerNpc)
        {
            foreach (var record in RegisteredDialogues)
            {
                if (record.dialogue.Talker.Equals(talkerNpc))
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<Npc> GetAllRegisteredTalkers()
        {
            talkerSupportList ??= new();
            talkerSupportList.Clear();

            foreach (var record in RegisteredDialogues)
            {
                if (talkerSupportList.Contains(record.dialogue.Talker)) continue;

                talkerSupportList.Add(record.dialogue.Talker);
                yield return record.dialogue.Talker;
            }
        }

        public JToken CaptureAsJToken()
        {
            var recordsArray = new JArray();

            foreach (var record in RegisteredDialogues)
            {
                var recordObj = new JObject
                {
                    ["dialogueId"] = record.dialogue.Id,
                    ["clues"] = new JArray(record.collectedClues.Select(c => c.Id) ?? Enumerable.Empty<string>())
                };

                recordsArray.Add(recordObj);
            }

            return new JObject
            {
                ["dialogueRecords"] = recordsArray
            };
        }

        public void RestoreFromJToken(JToken state)
        {
            _registeredDialogues = new();

            if (state["dialogueRecords"] is not JArray recordsArray) return;

            var allDialogues = Resources.LoadAll<Dialogue>("Data/Dialogues").ToDictionary(d => d.Id, d => d);;
            var allClues = Resources.LoadAll<Clue>("Data/Clues").ToDictionary(c => c.Id, c => c);

            foreach (var recordToken in recordsArray)
            {
                var dialogueId = (string)recordToken["dialogueId"];
                var clueIds = recordToken["clues"]?.ToObject<List<string>>() ?? new();

                if (!allDialogues.TryGetValue(dialogueId, out var dialogue))
                {
                    Debug.LogWarning($"Dialogue with ID '{dialogueId}' not found in Resources/Data/Dialogues.");
                    continue;
                }

                var clueList = new List<Clue>();
                foreach (var clueId in clueIds)
                {
                    if (allClues.TryGetValue(clueId, out var clue))
                    {
                        clueList.Add(clue);
                    }
                    else
                    {
                        Debug.LogWarning($"Clue with ID '{clueId}' not found in Resources/Data/Clues.");
                    }
                }

                var record = new DialogueRecord
                {
                    dialogue = dialogue,
                    collectedClues = clueList
                };

                _registeredDialogues.Add(record);
            }
        }
    }
}